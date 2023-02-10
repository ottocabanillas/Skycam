using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArcamGUI : MonoBehaviour
{
    // Internal Properties
    public Text uiLabelVx, uiLabelVy, uiLabelVz;
    public GameObject uiVxObject, uiVyObject, uiVzObject;
    public SkycamController skycamController;
    private float[] ropeSpeeds = new float[4];
    private int _sendDataFrequency = 30; // Para enviar datos cada 30 frames
    private int _frameCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        skycamController = FindObjectOfType<SkycamController>();
        uiLabelVx = uiVxObject.GetComponent<Text>();
        uiLabelVy = uiVyObject.GetComponent<Text>();
        uiLabelVz = uiVzObject.GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        uiLabelVx.text = "Vx: " + skycamController._currentSpeed_X.ToString("N2");
        uiLabelVy.text = "Vy: " + skycamController._currentSpeed_Y.ToString("N2");
        uiLabelVz.text = "Vz: " + skycamController._currentSpeed_Z.ToString("N2");           

		_frameCounter++;
        
        if (_frameCounter % _sendDataFrequency == 0)
        {
            RopeSpeedFormatter.Instance.AddAxisVelocity(0, "Vx: " + skycamController._currentSpeed_X.ToString("N2"));
            RopeSpeedFormatter.Instance.AddAxisVelocity(1, "Vz: " + skycamController._currentSpeed_Z.ToString("N2"));
            RopeSpeedFormatter.Instance.AddAxisVelocity(2, "Vy: " + skycamController._currentSpeed_Y.ToString("N2"));
        }
    }
}
