using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class RopeSpeedFormatter : MonoBehaviour
{
    private static RopeSpeedFormatter instance;
	private float[] ropeSpeeds = new float[4];
	private string[] axisVelocities = new string[3];
    private int frameCounter;
    private int _sendDataFrequency = 30;

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
        ropeSpeeds = new float[4];
        frameCounter = 0;
    }

    public void AddRope(int ropeIndex, float ropeSpeed)
    {
        ropeSpeeds[ropeIndex] = ropeSpeed;
    }

	public void AddAxisVelocity(int position, string velocity) 
	{
		axisVelocities[position] = velocity;
	}

    private void Update()
    {
        frameCounter++;
        if (frameCounter % _sendDataFrequency == 0)
        {
			// Por ahora solo mandamos las velocidades de cada eje (X, Y, Z).
			// Despues vamos a volver a utilizar el metodo SendRopeSpeeds cuando implementemos el modelo
			
			SendAxisVelocities();
        }
    }

    public void SendRopeSpeeds()
    {
        // Concatenamos la lista de velocidades en un string separado por comas
        string speedsString = ropeSpeeds[0].ToString() + ", " + ropeSpeeds[1].ToString() + "," + ropeSpeeds[2].ToString() + ", " + ropeSpeeds[3].ToString();
        // Enviamos el string de velocidades a la placa
        ArduinoController.Instance.SendValue(speedsString);
    }

	public void SendAxisVelocities() 
	{
		string velocities = axisVelocities[0] + "," + axisVelocities[1] + "," + axisVelocities[2];
		ArduinoController.Instance.SendValue(velocities);	
	}
}