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

    public void AddRope(int ropeIndex, float ropeSpeed)
    {
        ropeSpeeds[ropeIndex] = ropeSpeed;
    }

    private void Update()
    {
        // Calcular todo el tiempo la direccion de cada cuerda
        CalculateRopesDiff();
        // Calcular velocidad para los motores de Batcam
        CalculateMotorsVelocities();

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

    // public void SendRopeSpeeds()
    // {
    //     // Concatenamos la direccion de cada cuerda y su velocidad.
    //     string payload = ropeDirections[0] + g_variables.v1.ToString() +
    //                      ropeDirections[1] + g_variables.v2.ToString() +
    //                      ropeDirections[2] + g_variables.v3.ToString() +
    //                      ropeDirections[3] + g_variables.v4.ToString() + "*";

    //     //Debug.Log("DATOS ENVIADOS: " + payload);
    //     ArduinoController.Instance.SendValue(payload);
    // }

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
        //ropeDirections[ropeIndex] = direction;
    }

    private void ParseIncomingDataFromArgosUc()
    {
        // Comenzamos a parsear, caracter por caracter.
        CentralUnitParser.Instance.ProcessInput();

        //Enviamos las longitudes al modelo de cinematica directa, solo si el estado de las 4 VMUs es OK!
        if (CentralUnitParser.isSkycamStatusOk)
        {
            Debug.Log("Central Unit stat OK");

            // Calculamos los valores X,Y,Z reales con el modelo de cinematica directa
            DirectKinematic.Instance.SetLengthsAndCalculateXYZ();
        }
        else
        {
            //Manejar errores? Definirlo con Otto
            //Debug.Log("Central Unit stat not OK");
        }
    }

    /* Funcion encargada de calcular la diferencia entre los largos reales y largos deseados de cada cuerda
    */
    private void CalculateRopesDiff()
    {
        g_variables.d1 = (float)g_variables.R1 - g_variables.sp1;
        g_variables.d2 = (float)g_variables.R2 - g_variables.sp2;
        g_variables.d3 = (float)g_variables.R3 - g_variables.sp3;
        g_variables.d4 = (float)g_variables.R4 - g_variables.sp4;
    }

    /* Funcion encargada de calcular la velocidad de giro de cada motor
    */
    private void CalculateMotorsVelocities()
    {
        g_variables.v1 = Math.Round((g_variables.d1 / g_variables.T), MidpointRounding.AwayFromZero);
        g_variables.v2 = Math.Round((g_variables.d2 / g_variables.T), MidpointRounding.AwayFromZero);
        g_variables.v3 = Math.Round((g_variables.d3 / g_variables.T), MidpointRounding.AwayFromZero);
        g_variables.v4 = Math.Round((g_variables.d4 / g_variables.T), MidpointRounding.AwayFromZero);
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
            yield return new WaitForSeconds(0.2f);
        }
    }

    /* Funcion encargada de mandar los comandos requeridos por ArgosUC
    */
    private IEnumerator SendCommands()
    {
        // Enviar comando siempre que la diferencia sea mayor a 10 mm 
        while (g_variables.d > 0.10f)
        {
            string payload = ropeDirections[0] + g_variables.v1.ToString() +
                             ropeDirections[1] + g_variables.v2.ToString() +
                             ropeDirections[2] + g_variables.v3.ToString() +
                             ropeDirections[3] + g_variables.v4.ToString() + "*";

            //Debug.Log("DATOS ENVIADOS: " + payload);
            ArduinoController.Instance.SendValue(payload);
            yield return new WaitForSeconds(0.2f);
        }
    }
}