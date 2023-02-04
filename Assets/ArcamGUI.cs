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
    // Start is called before the first frame update
    void Start()
    {
        skycamController = FindObjectOfType<SkycamController>();
        uiLabelVx = uiVxObject.GetComponent<Text>();
        uiLabelVy = uiVyObject.GetComponent<Text>();
        uiLabelVz = uiVzObject.GetComponent<Text>();
        
        Debug.Log(skycamController._currentSpeed_X.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(skycamController._currentSpeed_X.ToString());
        uiLabelVx.text = "Vx: " + skycamController._currentSpeed_X.ToString("N2");
        uiLabelVy.text = "Vy: " + skycamController._currentSpeed_Y.ToString("N2");
        uiLabelVz.text = "Vz: " + skycamController._currentSpeed_Z.ToString("N2");
    }
}
