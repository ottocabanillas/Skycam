using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamareController : MonoBehaviour
{
    public GameObject[] listaCamaras;
    // Start is called before the first frame update
    void Start()
    {
        listaCamaras[0].gameObject.SetActive(true);
        listaCamaras[1].gameObject.SetActive(false);
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
            }

            if (Gamepad.current.buttonWest.isPressed || Input.GetKey(KeyCode.Alpha2))
            {
                listaCamaras[0].gameObject.SetActive(false);
                listaCamaras[1].gameObject.SetActive(true);
            }
        } else {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                listaCamaras[0].gameObject.SetActive(true);
                listaCamaras[1].gameObject.SetActive(false);
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                listaCamaras[0].gameObject.SetActive(false);
                listaCamaras[1].gameObject.SetActive(true);
            }


        }
    }
}
