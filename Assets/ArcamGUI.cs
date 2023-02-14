using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArcamGUI : MonoBehaviour
{
    // Internal Properties
    public Text  cameraPosition,
                 skycamHeight,
                 skycamSpeed;
    public GameObject cameraObject,
                      skycamObject;
    public CameraController cameraController;
    public SkycamController skycamController;
    // Start is called before the first frame update
    void Start()
    {
        cameraController = FindAnyObjectByType<CameraController>();
        cameraPosition = cameraObject.GetComponent<Text>();
        //skycamHeight.text = skycamController.currentHeight;
        //skycamSpeed = skycamObject.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        cameraPosition.text = "Posicion: " + cameraController.cameraName;
        skycamHeight.text = "Altura: " + skycamController.currentHeight;
        skycamSpeed.text = "Velocidad: " + skycamController.currentSpeed;
        //uiLabelVx.text = "Vx: " + skycamController._currentSpeed_X.ToString("N2");
    }
}
