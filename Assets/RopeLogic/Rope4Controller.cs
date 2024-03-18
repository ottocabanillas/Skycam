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

    void Start()
    {
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

        // Update the rope length as the cube moves
        ropeLength = Vector3.Distance(rightTopVertex, ropePole);

        sp4Text.SetText("SP4: " + (ropeLength * 1000).ToString("N0") + " mm");

        // Determinar "F" o "R" de acuerdo al largo anterior y el largo actuals
        RopeSpeedFormatter.Instance.RopeDirectionParser(
            ropeLength, 
            previousRopeLength,
            ropeIndex: 3
        );

        previousRopeLength = ropeLength;
    }
}
