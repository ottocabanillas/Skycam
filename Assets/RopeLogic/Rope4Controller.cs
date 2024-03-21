using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rope4Controller : MonoBehaviour
{
    public GameObject cube,
        rope,
        pole; // Para asignar Skycam, cuerda y poste desde el inspector

    [SerializeField]
    private TMP_Text sp4Text; // Texto para mostrar el largo deseado L4

    private Vector3 rightTopVertex,
        ropePole,
        poleTop;
    private float ropeLength, previousRopeLength;
    private LineRenderer lineRenderer;

    private GlobalVariables g_variables; 

    void Start()
    {
        // Instancia unica de la clase para almacenar las variables globales
        g_variables = GlobalVariables.Instance;
        g_variables.mt4 = new Vector3(3.15f, 0f, 2.28f);

        lineRenderer = rope.GetComponent<LineRenderer>();
        ropePole = pole.transform.TransformPoint(new Vector3(-0.35f, 1.00f, 0.35f));
        lineRenderer.SetPosition(1, ropePole);
        
        rightTopVertex = cube.transform.TransformPoint(new Vector3(0.25f, 0.25f, -0.25f));
        previousRopeLength = Vector3.Distance(rightTopVertex, ropePole);
    }

    void Update()
    {
        // Update the rope's end position to match the left top vertex of the cube
        rightTopVertex = cube.transform.TransformPoint(new Vector3(0.25f, 0.25f, -0.25f));

        lineRenderer.SetPosition(0, rightTopVertex);
        //lineRenderer.SetPosition(1, ropePole);

        Vector3 centroCamara = new Vector3(cube.transform.position.x, cube.transform.position.z, cube.transform.position.y);
        
        // Actualizar el largo de la cuerda 4 mientras la Skycam se mueve
        ropeLength = Vector3.Distance(g_variables.mt4, centroCamara);
        g_variables.sp4 = ropeLength;

        sp4Text.SetText("SP4: " + (g_variables.sp4 * 1000).ToString("N0") + " mm");

        // Determinar "F" o "R" de acuerdo al largo anterior y el largo actuals
        //RopeSpeedFormatter.Instance.RopeDirectionParser(
        //    ropeLength, 
        //    previousRopeLength,
        //    ropeIndex: 3
        //);

        previousRopeLength = ropeLength;
    }
}
