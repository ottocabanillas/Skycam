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

    private Vector3 rearRightTopVertex,
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
        // ropePole = pole.transform.TransformPoint(new Vector3(0.5f, 0.5f, -0.5f));
        // lineRenderer.SetPosition(0, ropePole);

        rearRightTopVertex = cube.transform.TransformPoint(new Vector3(-0.25f, 0.25f, 0.25f));
        previousRopeLength = Vector3.Distance(rearRightTopVertex, ropePole);
    }

    void Update()
    {
        ropePole = pole.transform.TransformPoint(new Vector3(0.35f, 1.00f, -0.35f));
        lineRenderer.SetPosition(0, ropePole);
        // Update the rope's end position to match the left top vertex of the cube
        rearRightTopVertex = cube.transform.TransformPoint(new Vector3(-0.5f, 0.5f, 0.5f));
        lineRenderer.SetPosition(1, rearRightTopVertex);

        Vector3 centroCamara = new Vector3(cube.transform.position.x, cube.transform.position.y, cube.transform.position.z);

        // Actualizar el largo de la cuerda 2 mientras la Skycam se mueve
        // ropeLength = Vector3.Distance(g_variables.mt2, centroCamara);
        // g_variables.sp2 = ropeLength;

        ropeLength = Vector3.Distance(rearRightTopVertex, ropePole);
        g_variables.sp2 = ropeLength;
        // Debug.Log("Cuerda 2: " + ropeLength);
    }
}
