using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class RopeSpeedFormatter : MonoBehaviour
{
    private static RopeSpeedFormatter instance;
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
    private float[] ropeSpeeds = new float[4];
    private int frameCounter;
    private int _sendDataFrequency = 30;
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
        ropeSpeeds = new float[4];
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
            bool allSpeedsSet = true;
            for (int i = 0; i < 4; i++)
            {
                if (ropeSpeeds[i] == 0)
                {
                    allSpeedsSet = false;
                    break;
                }
            }
            if (allSpeedsSet)
            {
                SendRopeSpeeds();
            }
        }
    }
    public void SendRopeSpeeds()
    {
        // Concatenamos la lista de velocidades en un string separado por comas
        string speedsString = ropeSpeeds[0].ToString() + "," + ropeSpeeds[1].ToString() + "," + ropeSpeeds[2].ToString() + "," + ropeSpeeds[3].ToString();
        // Enviamos el string de velocidades a la placa
        ArduinoController.Instance.SendValue(speedsString);
    }
}