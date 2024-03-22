using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class RopeSpeedFormatter : MonoBehaviour
{
    private static RopeSpeedFormatter instance;
    private float[] ropeSpeeds = new float[4];
    public string[] motorsDirections = new string[4];
    private GlobalVariables g_variables;
    private SkycamController skycamController;

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

        // Iniciar la corutina para mandar constantemente el TTL
        StartCoroutine(SendTimeToLivePeriodically());
        StartCoroutine(SendCommands());
        skycamController = FindAnyObjectByType<SkycamController>();
    }

    private void Update()
    {
        // Calcular D1, D2, D3 Y D4
        g_variables.CalculateRopesDiff();

        // Calcular la velocidad de giro de cada motor
        g_variables.CalculateMotorVelocities();

        // Establecer direcciones de acuerdo a las veloc de los motores
        g_variables.SetMotorsDirections();

        if (!ArduinoController.isSerialConnEstablished)
        {
            // No se establecio la conexion a traves del puerto serie, retornar
            //Debug.Log("Serial conn not established!");
            return;
        }

        // Leemos constantemente el largo de los 4 VMUs y sus respectivos estados
        ArduinoController.Instance.ReadSerialPortData();
        // Parseamos la respuesta
        ParseIncomingDataFromArgosUc();
    }

    private void ParseIncomingDataFromArgosUc()
    {
        // Comenzar a parsear, caracter por caracter.
        CentralUnitParser.Instance.ProcessInput();

        //Enviamos las longitudes al modelo de cinematica directa, solo si el estado de las 4 VMUs es OK!
        if (CentralUnitParser.isSkycamStatusOk)
        {
            // Calculamos los valores X,Y,Z reales con el modelo de cinematica directa
            DirectKinematic.Instance.SetLengthsAndCalculateXYZ(
                (double)g_variables.R1 / 1000.0,
                (double)g_variables.R2 / 1000.0,
                (double)g_variables.R3 / 1000.0,
                (double)g_variables.R4 / 1000.0
            );

        }
        else
        {
            //Manejar errores? Definirlo...
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
        while (ArduinoController.isSerialConnEstablished)
        {
            SendTimeToLive();
            yield return new WaitForSeconds(0.1f);
        }
    }

    /* Funcion encargada de mandar los comandos requeridos por ArgosUC
    */
    private IEnumerator SendCommands()
    {
        while (ArduinoController.isSerialConnEstablished)
        {
            string payload = g_variables.motorsDirections[0] + Math.Abs(g_variables.v1).ToString() +
                             g_variables.motorsDirections[1] + Math.Abs(g_variables.v2).ToString() +
                             g_variables.motorsDirections[2] + Math.Abs(g_variables.v3).ToString() +
                             g_variables.motorsDirections[3] + Math.Abs(g_variables.v4).ToString() + "*";

            //Debug.Log("Enviando: " + payload);
            // Enviar comando siempre que la diferencia sea mayor a 10 mm 
            if (g_variables.d > 0.01)
            {
                Debug.Log("Enviando: " + payload);
                ArduinoController.Instance.SendValue(payload);  
            }
            //Debug.Log("DATOS ENVIADOS: " + payload);
            yield return new WaitForSeconds(0.1f);
        }
    }
}