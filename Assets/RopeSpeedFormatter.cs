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
    private int _sendDataFrequency = 15;


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
        if (!ArduinoController.isSerialConnEstablished)
        {
            // No se establecio la conexion a traves del puerto serie, retornar
            Debug.Log("Serial conn not established!");
            return;
        }

        SendRopeSpeeds();

        // Leemos la respuesta de Argos UC
        ArduinoController.Instance.ReadSerialPortData();
        // Parseamos la respuesta
        ParseIncomingDataFromArgosUc();
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
    private void ParseIncomingDataFromArgosUc()
    {
        // Comenzamos a parsear, caracter por caracter.
        CentralUnitParser.Instance.ProcessInput();

        //Enviamos las longitudes al modelo de cinematica inversa
        // solo si el estado de las 4 VMUs es OK!
        if (CentralUnitParser.isSkycamStatusOk)
        {

            Debug.Log("Central Unit stat OK");
            Debug.Log(CentralUnitParser.Instance.m_vmuLengthArr[0]);
            Debug.Log(CentralUnitParser.Instance.m_vmuLengthArr[1]);
            Debug.Log(CentralUnitParser.Instance.m_vmuLengthArr[2]);
            Debug.Log(CentralUnitParser.Instance.m_vmuLengthArr[3]);

            // Pasamos las 4 longitudes al constructor del modelo.
            DirectKinematic.Instance.Initialize(
                CentralUnitParser.Instance.m_vmuLengthArr[0] / 1000,
                CentralUnitParser.Instance.m_vmuLengthArr[1] / 1000,
                CentralUnitParser.Instance.m_vmuLengthArr[2] / 1000,
                CentralUnitParser.Instance.m_vmuLengthArr[3] / 1000
            );

            // TODO: Hacer algo con los valores X,Y,Z del modelo matematico

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