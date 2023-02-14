using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArcamGUI : MonoBehaviour
{
    // Internal Properties
    public Text cameraPosition;
    public GameObject cameraObject;
    public CameraController cameraController;
    // Start is called before the first frame update
    void Start()
    {
        cameraController = FindAnyObjectByType<CameraController>();
        cameraPosition = cameraObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosition.text = "Posicion: " + cameraController.cameraName;
        //uiLabelVx.text = "Vx: " + skycamController._currentSpeed_X.ToString("N2");
    }
}
