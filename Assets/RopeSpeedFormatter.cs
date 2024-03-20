using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class RopeSpeedFormatter : MonoBehaviour
{
    private static RopeSpeedFormatter instance;
    private float[] ropeSpeeds = new float[4];
    public string[] ropeDirections = new string[4];
    private GlobalVariables g_variables;

    // Flag para determinar si la skycam ya se encuentra posicionada
    private bool _IsSkycamPositioned;
    private SkycamController skycamController;
    public bool IsSkycamPositioned
    {
        get { return _IsSkycamPositioned; }
        set { _IsSkycamPositioned = value; }
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
        g_variables = GlobalVariables.Instance;
        _IsSkycamPositioned = false;

        // Iniciar la corutina para mandar constantemente el TTL
        StartCoroutine(SendTimeToLivePeriodically());
        StartCoroutine(SendCommands());
        skycamController = FindAnyObjectByType<SkycamController>();
    }

    private void Update()
    {
        if (!ArduinoController.isSerialConnEstablished)
        {
            // No se establecio la conexion a traves del puerto serie, retornar
            Debug.Log("Serial conn not established!");
            return;
        }

        // Leemos constantemente el largo de los 4 VMUs y sus respectivos estados
        ArduinoController.Instance.ReadSerialPortData();
        // Parseamos la respuesta
        ParseIncomingDataFromArgosUc();
    }

    public void RopeDirectionParser(float currentLength, float previousLength, int ropeIndex)
    {
        string direction = "F"; // valor por defecto
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

    private void ParseIncomingDataFromArgosUc()
    {
        // Comenzamos a parsear, caracter por caracter.
        CentralUnitParser.Instance.ProcessInput();

        //Enviamos las longitudes al modelo de cinematica directa, solo si el estado de las 4 VMUs es OK!
        if (CentralUnitParser.isSkycamStatusOk)
        {
            //Debug.Log("Central Unit stat OK");

            // Calculamos los valores X,Y,Z reales con el modelo de cinematica directa
            DirectKinematic.Instance.SetLengthsAndCalculateXYZ(
                (double)g_variables.R1 / 1000.0,
                (double)g_variables.R2 / 1000.0,
                (double)g_variables.R3 / 1000.0,
                (double)g_variables.R4 / 1000.0
            );

            // Calcular las distancias entre los largos reales y los esperados
            g_variables.CalculateRopesDiff();

            // Calcular la velocidad de giro de cada motor
            g_variables.CalculateMotorVelocities();
        }
        else
        {
            //Manejar errores? Definirlo con Otto
            //Debug.Log("Central Unit stat not OK");
        }
    }

    public float RoundRopeDistance(float distance)
    {
        return MathF.Round(distance, 2);
    }

    private void SendTimeToLive()
    {
        ArduinoController.Instance.SendValue("*");
        return;
    }

    private IEnumerator SendTimeToLivePeriodically()
    {
        while (true)
        {
            SendTimeToLive();
            yield return new WaitForSeconds(0.1f);
        }
    }

    /* Funcion encargada de mandar los comandos requeridos por ArgosUC
    */
    private IEnumerator SendCommands()
    {
        // Enviar comando siempre que la diferencia sea mayor a 10 mm 
        while (true)
        {
            string payload = ropeDirections[0] + g_variables.v1.ToString() +
                             ropeDirections[1] + g_variables.v2.ToString() +
                             ropeDirections[2] + g_variables.v3.ToString() +
                             ropeDirections[3] + g_variables.v4.ToString() + "*";

            if (g_variables.d > 0.10)
            {
                Debug.Log("D: " + g_variables.d);
                Debug.Log("Enviando: " + payload);
                //ArduinoController.Instance.SendValue(payload);  
            }
            //Debug.Log("DATOS ENVIADOS: " + payload);
            yield return new WaitForSeconds(0.1f);
        }
    }
}