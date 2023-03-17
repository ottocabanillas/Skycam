using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    public GameObject poste1,poste2, poste3, poste4;
    public GameObject skycamOtto;
    public GameObject floor;
    public GameObject camFront, camUp, camIsometric;
    private float trueLength = GUIConfigurationController.lengthValue,
        trueWidth = GUIConfigurationController.widthValue,
        trueHeight = GUIConfigurationController.heightValue;

    // Start is called before the first frame update
    void Start()
    {
        trueLength = 10f;
        trueWidth = 5.00f;
        trueHeight = 2.00f;
        configHeightPole();
        configFloor();
    }

    // Update is called once per frame
    void Update() { }

    void configFloor() {
        floor.transform.localScale = new Vector3(trueLength*2, floor.transform.localScale.y, trueWidth*2);
        floor.transform.position = new Vector3(trueLength, floor.transform.position.y, trueWidth);
    }

    void configHeightPole() {
        configHeight(poste3, trueHeight);
        configHeight(poste4, trueHeight);
        configHeight(poste1, trueHeight);
        configHeight(poste2, trueHeight);
    }

    void configHeight (GameObject poste, float value) {
        poste.transform.localScale = new Vector3(
            poste.transform.localScale.x,
            trueHeight,
            poste.transform.localScale.z
        );
        poste.transform.position = new Vector3(
            poste.transform.position.x,
            trueHeight,
            poste.transform.position.z
        );
    }
}
