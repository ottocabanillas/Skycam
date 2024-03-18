using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PilotInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text sp_xText; // Texto para mostrar el punto x deseado

    [SerializeField]
    private TMP_Text sp_yText; // Texto para mostrar el punto y deseado

    [SerializeField]
    private TMP_Text sp_zText; // Texto para mostrar el punto z deseado

    [SerializeField]
    private TMP_Text r1Text; // Texto para mostrar el largo real de cuerda de VMU1

    [SerializeField]
    private TMP_Text r2Text; // Texto para mostrar el largo real de cuerda de VMU2

    [SerializeField]
    private TMP_Text r3Text; // Texto para mostrar el largo real de cuerda de VMU3

    [SerializeField]
    private TMP_Text r4Text; // Texto para mostrar el largo real de cuerda de VMU4

    [SerializeField]
    private GameObject skycam; // Skycam

    private DirectKinematic directKinematicModel;

    //private GlobalVariables g_variables = GlobalVariables.instance;

    private Vector3 desiredPosition;

    // Start is called before the first frame update
    void Start()
    {
        directKinematicModel = DirectKinematic.Instance;
        desiredPosition = skycam.transform.position;
        sp_xText.SetText("SPx: " + (desiredPosition.x * 1000).ToString("N0") + " mm");
        sp_yText.SetText("SPy: " + (desiredPosition.z * 1000).ToString("N0") + " mm");
        sp_zText.SetText("SPz: " + (desiredPosition.y * 1000).ToString("N0") + " mm");

        // Largos reales de cuerdas emitidos por ArgosUC
        r1Text.SetText("R1: " + (directKinematicModel.L1 * 1000).ToString("N0") + " mm");
        r2Text.SetText("R2: " + (directKinematicModel.L2 * 1000).ToString("N0") + " mm");
        r3Text.SetText("R3: " + (directKinematicModel.L3 * 1000).ToString("N0") + " mm");
        r4Text.SetText("R4: " + (directKinematicModel.L4 * 1000).ToString("N0") + " mm");
    }

    // Update is called once per frame
    void Update()
    {
        desiredPosition = skycam.transform.position;
        // Posicion deseada X,Y,Z
        sp_xText.SetText("SPx: " + (desiredPosition.x * 1000).ToString("N0") + " mm");
        sp_yText.SetText("SPy: " + (desiredPosition.z * 1000).ToString("N0") + " mm");
        sp_zText.SetText("SPz: " + (desiredPosition.y * 1000).ToString("N0") + " mm");

        // Largos reales de cuerdas emitidos por ArgosUC
        r1Text.SetText("R1: " + (directKinematicModel.L1 * 1000).ToString("N0") + " mm");
        r2Text.SetText("R2: " + (directKinematicModel.L2 * 1000).ToString("N0") + " mm");
        r3Text.SetText("R3: " + (directKinematicModel.L3 * 1000).ToString("N0") + " mm");
        r4Text.SetText("R4: " + (directKinematicModel.L4 * 1000).ToString("N0") + " mm");
    }

}
