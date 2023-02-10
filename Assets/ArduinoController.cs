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

    [SerializeField]
    private string portName;
    [SerializeField]
    private int baudRate = 9600;

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
        serialPort.WriteLine(value);
    }
}