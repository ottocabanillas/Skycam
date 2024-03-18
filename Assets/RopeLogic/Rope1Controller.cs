using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class Rope1Controller : MonoBehaviour
{
    public GameObject cube,
        rope,
        pole; // Para asignar Skycam, cuerda y poste desde el inspector

    private Vector3 rightTopVertex,
        ropePole,
        poleTop;
    private float ropeLength, previousRopeLength;
    private LineRenderer lineRenderer;

    private Vector3 m_t1;
    //private GlobalVariables g_variables; 

    [SerializeField]
    private TMP_Text sp1Text; // Texto para mostrar el largo deseado L1

    void Start()
    {
        //g_variables = GlobalVariables.Instance;
        
        m_t1 = new Vector3(0f,0f,2.28f);
        lineRenderer = rope.GetComponent<LineRenderer>();
        ropePole = pole.transform.TransformPoint(new Vector3(0.35f, 1.00f, 0.35f));
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
        // Actualizar el largo de la cuerda mientras la Skycam se mueve
        ropeLength = Vector3.Distance(m_t1, centroCamara);

        sp1Text.SetText("SP1: " + (ropeLength * 1000).ToString("N0") + " mm");
        // Aca establecemos F o R dependiendo si se debe soltar o contraer cuerda
        // antes de pasarle el largo actual y el largo anterior redondeamos a dos decimales.
        RopeSpeedFormatter.Instance.RopeDirectionParser(
                ropeLength,
                previousRopeLength,
                ropeIndex: 0
        );

        // Actualizo el valor del previousRopeLength
        previousRopeLength = ropeLength;
    }
}
