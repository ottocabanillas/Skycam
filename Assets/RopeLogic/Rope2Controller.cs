using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rope2Controller : MonoBehaviour
{
    public GameObject cube,
        rope,
        pole; // Para asignar Skycam, cuerda y poste desde el inspector
    
    [SerializeField]
    private TMP_Text sp2Text; // Texto para mostrar el largo deseado L2

    private Vector3 rightTopVertex,
        ropeEnd,
        ropePole,
        poleTop;
    private float ropeLength, previousRopeLength;
    private LineRenderer lineRenderer;

    private GlobalVariables g_variables; 

    void Start()
    {
        // Instancia unica de la clase para almacenar las variables globales
        g_variables = GlobalVariables.Instance;
        g_variables.mt2 = new Vector3(0f, 1.85f, 2.28f);
        
        lineRenderer = rope.GetComponent<LineRenderer>();
        ropePole = pole.transform.TransformPoint(new Vector3(0.35f, 1.00f, -0.35f));
        lineRenderer.SetPosition(1, ropePole);

        rightTopVertex = cube.transform.TransformPoint(new Vector3(-0.25f, 0.25f, 0.25f));
        previousRopeLength = Vector3.Distance(rightTopVertex, ropePole);
    }

    void Update()
    {
        // Update the rope's end position to match the left top vertex of the cube
        rightTopVertex = cube.transform.TransformPoint(new Vector3(-0.25f, 0.25f, 0.25f));
        lineRenderer.SetPosition(0, rightTopVertex);

        Vector3 centroCamara = new Vector3(cube.transform.position.x, cube.transform.position.y, cube.transform.position.z);

        // Actualizar el largo de la cuerda 2 mientras la Skycam se mueve
        ropeLength = Vector3.Distance(g_variables.mt2, centroCamara);
        g_variables.sp2 = ropeLength;

        sp2Text.SetText("SP2: " + (g_variables.sp2 * 1000).ToString("N0") + " mm");

        // Determinar "F" o "R" de acuerdo al largo anterior y el largo actual
        //RopeSpeedFormatter.Instance.RopeDirectionParser(
        //    ropeLength,
        //    previousRopeLength,
        //    ropeIndex: 1
        //);

        // Actualizamos el valor previo
        previousRopeLength = ropeLength;
    }
}
