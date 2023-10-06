using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject[] listaCamaras;
    public string cameraName;
    // Start is called before the first frame update
    void Start()
    {
        listaCamaras[0].gameObject.SetActive(true);
        listaCamaras[1].gameObject.SetActive(false);
        listaCamaras[2].gameObject.SetActive(false);
        cameraName = "Cámara Frontal";
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.isPressed || Input.GetKey(KeyCode.Alpha1))
            {
                listaCamaras[0].gameObject.SetActive(true);
                listaCamaras[1].gameObject.SetActive(false);
                listaCamaras[2].gameObject.SetActive(false);
                cameraName = "Cámara Frontal";
            }

            if (Gamepad.current.buttonWest.isPressed || Input.GetKey(KeyCode.Alpha2))
            {
                listaCamaras[0].gameObject.SetActive(false);
                listaCamaras[1].gameObject.SetActive(true);
                listaCamaras[2].gameObject.SetActive(false);
                cameraName = "Cámara Superior";
            }
            if (Gamepad.current.buttonNorth.isPressed || Input.GetKey(KeyCode.Alpha3))
            {
                listaCamaras[0].gameObject.SetActive(false);
                listaCamaras[1].gameObject.SetActive(false);
                listaCamaras[2].gameObject.SetActive(true);
                cameraName = "Cámara Isométrica";
            }
        } else {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                listaCamaras[0].gameObject.SetActive(true);
                listaCamaras[1].gameObject.SetActive(false);
                listaCamaras[2].gameObject.SetActive(false);
                cameraName = "Cámara Frontal";
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                listaCamaras[0].gameObject.SetActive(false);
                listaCamaras[1].gameObject.SetActive(true);
                listaCamaras[2].gameObject.SetActive(false);
                cameraName = "Cámara Superior";
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                listaCamaras[0].gameObject.SetActive(false);
                listaCamaras[1].gameObject.SetActive(false);
                listaCamaras[2].gameObject.SetActive(true);
                cameraName = "Cámara Isométrica";
            }
        }
    }
}
