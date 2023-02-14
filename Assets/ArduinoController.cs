using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

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
    private int baudRate = 115200;

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
        
        portName = GetConnectedArduinoPort();
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

    public string ReadSerialPortData()
    {
        string data = "";
        if (serialPort.BytesToRead > 0) 
        {
            data = serialPort.ReadExisting();
        }

        return data;
    }

    private static string GetConnectedArduinoPort()
    {
        string[] ports = System.IO.Directory.GetFiles("/dev/", "cu.usbmodem*");
        foreach (string port in ports)
            {
                try
                {
                    SerialPort arduinoPort = new SerialPort(port, 9600);
                    arduinoPort.Open();
                    if (arduinoPort != null) {
                        arduinoPort.Close();
                        return port;
                    }
                       
                }
                catch (System.Exception)
                {
                    // No se encontro puerto
                }
            }
        return "";      
    }
}