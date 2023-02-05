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
    private string[] ropeSpeeds = new string[4];
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
        Debug.Log(skycamController._currentSpeed_X.ToString());
        uiLabelVx.text = "Vx: " + skycamController._currentSpeed_X.ToString("N2");
        uiLabelVy.text = "Vy: " + skycamController._currentSpeed_Y.ToString("N2");
        uiLabelVz.text = "Vz: " + skycamController._currentSpeed_Z.ToString("N2");           

        ropeSpeeds[0] = skycamController._currentSpeed_X.ToString("N2");
        ropeSpeeds[1] = skycamController._currentSpeed_Y.ToString("N2");
        ropeSpeeds[2] = skycamController._currentSpeed_Z.ToString("N2");
        
		_frameCounter++;
        
        if (_frameCounter % _sendDataFrequency == 0)
        {
			ArduinoController.Instance.SendValue(ropeSpeeds[0] + "," + ropeSpeeds[1] + "," + ropeSpeeds[2]);
        }
                              
    }
}
