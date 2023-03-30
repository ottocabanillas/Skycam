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

         // Debo parsear el string data asi se lo paso al modelo matematico
         // vamos a recibir una cadena en formato "Long1,Long2,Long3,Long4,statusCode,%"
        IsSkycamPositionReady = ParseArduinoResponse("10820,10100,4760,6130,0,%");
        }
    }

    public void SendRopeSpeeds()
    {
        // Concatenamos la direccion de cada cuerda y su velocidad.
        string payload = ropeDirections[0] + ropeSpeeds[0].ToString() + "," + 
                         ropeDirections[1] + ropeSpeeds[1].ToString() + "," +
                         ropeDirections[2] + ropeSpeeds[2].ToString() + "," + 
                         ropeDirections[3] + ropeSpeeds[3].ToString() + "*";
        
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
    // devuelve TRUE si se el statusCode es cero y se pueden cargar las longituedes
    // FALSE en caso contrario y la skycam no se moveria.
    private bool ParseArduinoResponse(string data)
    {
        //Debug.Log("captured arduino data: " + data);
        string[] values = data.Split(',');
        string longitud1 = values[0];
        string longitud2 = values[1];
        string longitud3 = values[2];
        string longitud4 = values[3];
        string batcamStatusCode = values[4];

        //Enviamos las longitudes al modelo de cinematica directa
        // solo si el statusCode es cero
        if (int.Parse(batcamStatusCode) == 0) 
        {
            Debug.Log(true);
            // Pasamos las 4 longitudes al constructor del modelo.
            // DirectKinematic.Instance.Initialize(
            //     L1: double.Parse(longitud1)/1000,
            //     L2: double.Parse(longitud2)/1000,
            //     L3: double.Parse(longitud3)/1000,
            //     L4: double.Parse(longitud4)/1000
            // );
            //Debug.Log("SKYCAM POSITIONS READY");
            return true;
        }
        else {
            //Manejar errores? Definirlo con Otto
            Debug.Log("SKYCAM POSITIONS not READY");
            return false; // cambiar a false
        }
    }


   public float RoundRopeDistance(float distance)
   {
    return MathF.Round(distance, 2);
   }
}