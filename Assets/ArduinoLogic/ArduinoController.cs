using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Text;


public class ArduinoController : MonoBehaviour
{
    private static ArduinoController instance;

    public static ArduinoController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ArduinoController>();
                if (instance == null)
                {
                    GameObject go = new GameObject("ArduinoController");
                    instance = go.AddComponent<ArduinoController>();
                }
            }
            return instance;
        }
    }


    private string portName;
    private int baudRate;

    public static bool isSerialConnEstablished = false;

    private SerialPort serialPort;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        baudRate = int.Parse(PlayerPrefs.GetString(CommonConfigKeys.BAUDIOS_ARDUINO_STRING.ToString()));
        portName = GetConnectedArduinoPort();
        //Debug.Log("Baud Rate " +baudRate);
    }

    private void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
    }

    private void OnDestroy()
    {
        serialPort.Close();
    }

    public void SendValue(string value)
    {
        serialPort.Write(value);
    }

    public void ReadSerialPortData()
    {

        if (CentralUnitParser.Instance.m_lastInputChar != '\0')
        {
            // caracter pendiente de procesar.
            return;
        }

        if (serialPort.BytesToRead <= 0)
        {
            // nada que leer del puerto serie
            return;
        }

        try
        {
            int byteRead = serialPort.ReadByte(); // Lee un byte
            CentralUnitParser.Instance.m_lastInputChar = (char)byteRead; // Convierte el byte a char
        }
        catch (TimeoutException)
        {
            Debug.Log("Serial read timeout");
            CentralUnitParser.Instance.m_lastInputChar = '\0';
        }
    }

    private static string GetConnectedArduinoPort()
    {
        // Define the port for Windows
        string targetPort = "COM3";

    // Depending on the platform, set the appropriate search pattern
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    // Windows-specific code
    string[] ports = System.IO.Ports.SerialPort.GetPortNames();
    foreach (string port in ports)
    {
        // Check if the current port is the target port (COM3)
        if (port == targetPort)
        {
            try
            {
                using (SerialPort arduinoPort = new SerialPort(port, 115200))
                {
                    arduinoPort.Open();
                    if (arduinoPort.IsOpen)
                    {
                        arduinoPort.Close();
                        isSerialConnEstablished = true;
                        return port;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("Error opening port " + port + ": " + ex.Message);
            }
        }
    }
    #elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
    // Unix-based systems specific code
    string[] ports = System.IO.Directory.GetFiles("/dev/", "cu.usbserial-0001");
    foreach (string port in ports)
    {
        try
        {
            using (SerialPort arduinoPort = new SerialPort(port, 115200))
            {
                arduinoPort.Open();
                if (arduinoPort.IsOpen)
                {
                    arduinoPort.Close();
                    isSerialConnEstablished = true;
                    return port;
                }
            }
        }
        catch (System.Exception ex)
        {
            isSerialConnEstablished = false;
            Debug.Log("Error opening port " + port + ": " + ex.Message);
        }
    }
#endif
        // If no ports found or error, return an empty string
        isSerialConnEstablished = false;
        return "";
    }

}