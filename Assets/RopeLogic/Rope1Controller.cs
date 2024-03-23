using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class Rope1Controller : MonoBehaviour
{
    [SerializeField]
    private TMP_Text sp1Text; // Texto para mostrar el largo deseado L1    
    public GameObject cube, rope_1, column_1; // Para asignar Skycam, cuerda y poste desde el inspector
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
        g_variables.mt1 = new Vector3(0f,0f,2.28f);

        lineRenderer = rope_1.GetComponent<LineRenderer>();
        ropePole = column_1.transform.TransformPoint(new Vector3(0.35f, 1.00f, 0.35f));
        lineRenderer.SetPosition(1, ropePole);

        rightTopVertex = cube.transform.TransformPoint(new Vector3(-0.25f, 0.25f, -0.25f));
        previousRopeLength = Vector3.Distance(rightTopVertex, ropePole);
    }

    void Update()
    {
        // Actualiza la posición del extremo de la cuerda para que coincida con el vértice superior izquierdo del cubo.
        rightTopVertex = cube.transform.TransformPoint(new Vector3(-0.25f, 0.25f, -0.25f));
        lineRenderer.SetPosition(0, rightTopVertex);

        Vector3 centroCamara = new Vector3(cube.transform.position.x, cube.transform.position.z, cube.transform.position.y);
        
        // Actualizar el largo de la cuerda 1 mientras la Skycam se mueve
        ropeLength = Vector3.Distance(g_variables.mt1, centroCamara);
        g_variables.sp1 = ropeLength; 

        sp1Text.SetText("SP1: " + (g_variables.sp1 * 1000).ToString("N0") + " mm");
    }
}
