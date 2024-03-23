using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rope4Controller : MonoBehaviour
{
    public GameObject cube, rope, pole; // Para asignar Skycam, cuerda y poste desde el inspector

    [SerializeField]
    private TMP_Text sp4Text; // Texto para mostrar el largo deseado L4
    private Vector3 rearLeftTopVertex, ropePole, poleTop;
    private float ropeLength, previousRopeLength;
    private LineRenderer lineRenderer;

    private GlobalVariables g_variables; 

    void Start()
    {
        // Instancia unica de la clase para almacenar las variables globales
        g_variables = GlobalVariables.Instance;
        g_variables.mt4 = new Vector3(3.15f, 0f, 2.28f);

        lineRenderer = rope.GetComponent<LineRenderer>();
        // ropePole = pole.transform.TransformPoint(new Vector3(-0.35f, 1.00f, 0.35f));
        // lineRenderer.SetPosition(0, ropePole);
        
        rearLeftTopVertex = cube.transform.TransformPoint(new Vector3(0.25f, 0.25f, -0.25f));
        previousRopeLength = Vector3.Distance(rearLeftTopVertex, ropePole);
    }

    void Update()
    {
        ropePole = pole.transform.TransformPoint(new Vector3(-0.35f, 1.00f, 0.35f));
        lineRenderer.SetPosition(0, ropePole);
        // Update the rope's end position to match the left top vertex of the cube
        rearLeftTopVertex = cube.transform.TransformPoint(new Vector3(0.5f, 0.5f, -0.5f));

        lineRenderer.SetPosition(1, rearLeftTopVertex);
        //lineRenderer.SetPosition(1, ropePole);

        Vector3 centroCamara = new Vector3(cube.transform.position.x, cube.transform.position.z, cube.transform.position.y);
        
        // Actualizar el largo de la cuerda 4 mientras la Skycam se mueve
        // ropeLength = Vector3.Distance(g_variables.mt4, centroCamara);
        // g_variables.sp4 = ropeLength;

        ropeLength = Vector3.Distance(rearLeftTopVertex, ropePole);
        g_variables.sp4 = ropeLength;
        // Debug.Log("Cuerda 4: " + ropeLength);
    }
}
