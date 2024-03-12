using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject[] listaCamaras;
    public string cameraName;

    public GameObject dialogPanel;

    // Start is called before the first frame update
    void Start()
    {
        listaCamaras[0].gameObject.SetActive(true);
        listaCamaras[1].gameObject.SetActive(false);
        listaCamaras[2].gameObject.SetActive(false);
        cameraName = "Camara Frontal";
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPanelVisible())
        {
            return;
        }

        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.isPressed || Input.GetKey(KeyCode.Alpha1))
            {
                listaCamaras[0].gameObject.SetActive(true);
                listaCamaras[1].gameObject.SetActive(false);
                listaCamaras[2].gameObject.SetActive(false);
                cameraName = "Camara Frontal";
            }

            if (Gamepad.current.buttonWest.isPressed || Input.GetKey(KeyCode.Alpha2))
            {
                listaCamaras[0].gameObject.SetActive(false);
                listaCamaras[1].gameObject.SetActive(true);
                listaCamaras[2].gameObject.SetActive(false);
                cameraName = "Camara Superior";
            }
            if (Gamepad.current.buttonNorth.isPressed || Input.GetKey(KeyCode.Alpha3))
            {
                listaCamaras[0].gameObject.SetActive(false);
                listaCamaras[1].gameObject.SetActive(false);
                listaCamaras[2].gameObject.SetActive(true);
                cameraName = "Camara Isometrica";
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                listaCamaras[0].gameObject.SetActive(true);
                listaCamaras[1].gameObject.SetActive(false);
                listaCamaras[2].gameObject.SetActive(false);
                cameraName = "Camara Frontal";
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                listaCamaras[0].gameObject.SetActive(false);
                listaCamaras[1].gameObject.SetActive(true);
                listaCamaras[2].gameObject.SetActive(false);
                cameraName = "Camara Superior";
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                listaCamaras[0].gameObject.SetActive(false);
                listaCamaras[1].gameObject.SetActive(false);
                listaCamaras[2].gameObject.SetActive(true);
                cameraName = "Camara Isometrica";
            }
        }
    }

    public bool IsPanelVisible()
    {
        return dialogPanel.activeSelf;
    }
}
