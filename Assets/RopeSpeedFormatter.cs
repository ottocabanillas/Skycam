using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class RopeSpeedFormatter : MonoBehaviour
{
    private static RopeSpeedFormatter instance;
    private float[] ropeSpeeds = new float[4];
    public string[] ropeDirections = new string[4];
    private int frameCounter;
    private int _sendDataFrequency = 30;

    // Flag para saber si la skycam puede
    // utilizar los valores calculados de la cinematica directa.
    private bool isSkycamPositionReady = true;

    private SkycamController skycamController;
    public bool IsSkycamPositionReady
    {
        get { return isSkycamPositionReady; }
        set { isSkycamPositionReady = value; }
    }
    public static RopeSpeedFormatter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RopeSpeedFormatter>();
                if (instance == null)
                {
                    GameObject go = new GameObject("RopeSpeedFormatter");
                    instance = go.AddComponent<RopeSpeedFormatter>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        frameCounter = 0;
        skycamController = FindAnyObjectByType<SkycamController>();
    }

    public void AddRope(int ropeIndex, float ropeSpeed)
    {
        ropeSpeeds[ropeIndex] = ropeSpeed;
    }

    private void Update()
    {
        frameCounter++;
        if (frameCounter % _sendDataFrequency == 0)
        {
            frameCounter = 0;
            SendRopeSpeeds();

            //Aca leemos datos del puerto serie siempre que haya datos disponibles.
            string data = ArduinoController.Instance.ReadSerialPortData();
            Debug.Log(data);

            // Debemos parsear el string data asi se lo pasamos al modelo matematico
            // data es un String en formato "Long1,Long2,Long3,Long4,statusCode,%"
            ParseArduinoResponse(data);
        }
    }

    public void SendRopeSpeeds()
    {
        // Concatenamos la direccion de cada cuerda y su velocidad.
        string payload = ropeDirections[0] + GenerarValorAleatorio().ToString() + "," +
                         ropeDirections[1] + GenerarValorAleatorio().ToString() + "," +
                         ropeDirections[2] + GenerarValorAleatorio().ToString() + "," +
                         ropeDirections[3] + GenerarValorAleatorio().ToString() + "*";

        //Debug.Log("DATOS ENVIADOS: " + payload);
        ArduinoController.Instance.SendValue(payload);
    }
    public void RopeDirectionParser(float currentLength, float previousLength, int ropeIndex)
    {
        string direction = "";
        if (currentLength > previousLength)
        {
            direction = "F";
        }
        else if (currentLength < previousLength)
        {
            direction = "R";
        }
        ropeDirections[ropeIndex] = direction;
    }

    //Funcion para parsear lo que nos envia el sistema BatCam
    private void ParseArduinoResponse(string data)
    {
        if (data == "")
        {
            return;
        }

        string[] values = data.Split(',');
        // Chequear si el array tiene por lo menos 5 valores (4 longitudes y 1 status code)
        if (values.Length < 5)
        {
            Debug.Log("Formato incorrecto");
            return; // Salimos de la funcion ya que no podemos parsear.
        }

        string longitud1 = values[0];
        string longitud2 = values[1];
        string longitud3 = values[2];
        string longitud4 = values[3];
        string batcamStatusCode = values[4];

        Debug.Log("Parsed values: L1=" + longitud1);
        Debug.Log("Parsed values: L2=" + longitud2);
        Debug.Log("Parsed values: L3=" + longitud3);
        Debug.Log("Parsed values: L4=" + longitud4);
        Debug.Log("Parsed values: statCode=" + batcamStatusCode);


        //Enviamos las longitudes al modelo de cinematica inversa
        // solo si el statusCode es cero
        if (int.Parse(batcamStatusCode) == 0)
        {
            // Pasamos las 4 longitudes al constructor del modelo.
            DirectKinematic.Instance.Initialize(
                double.Parse(longitud1) / 1000,
                double.Parse(longitud2) / 1000,
                double.Parse(longitud3) / 1000,
                double.Parse(longitud4) / 1000
            );
            //Debug.Log("SKYCAM POSITIONS READY");
        }
        else
        {
            //Manejar errores? Definirlo con Otto
            //Debug.Log("SKYCAM POSITIONS not READY");
        }
    }

    public int GenerarValorAleatorio()
    {
        return skycamController.IsCameraStopped() ? 0 : UnityEngine.Random.Range(0, 256);
    }
    public float RoundRopeDistance(float distance)
    {
        return MathF.Round(distance, 2);
    }
}