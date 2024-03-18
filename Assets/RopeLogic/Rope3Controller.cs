using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rope3Controller : MonoBehaviour
{
    public GameObject cube,
        rope,
        pole;

    [SerializeField]
    private TMP_Text sp3Text; // Texto para mostrar el largo deseado L3

    private Vector3 rightTopVertex,
        ropePole,
        poleTop;
    private float ropeLength, previousRopeLength;
    private LineRenderer lineRenderer;

    private Vector3 m_t3;
    void Start()
    {
        m_t3 = new Vector3(3.15f, 1.85f, 2.28f);
        lineRenderer = rope.GetComponent<LineRenderer>();
        ropePole = pole.transform.TransformPoint(new Vector3(-0.35f, 1.00f, -0.35f));
        lineRenderer.SetPosition(1, ropePole);

        rightTopVertex = cube.transform.TransformPoint(new Vector3(0.25f, 0.25f, 0.25f));
        previousRopeLength = Vector3.Distance(rightTopVertex, ropePole);
    }

    void Update()
    {
        // Update the rope's end position to match the left top vertex of the cube
        rightTopVertex = cube.transform.TransformPoint(new Vector3(0.25f, 0.25f, 0.25f));
        lineRenderer.SetPosition(0, rightTopVertex);
        //lineRenderer.SetPosition(1, ropePole);

        // Update the rope length as the cube moves
        ropeLength = Vector3.Distance(rightTopVertex, ropePole);

        Vector3 centroCamara = new Vector3(cube.transform.position.x, cube.transform.position.z, cube.transform.position.y);

        sp3Text.SetText("SP3: " + (ropeLength * 1000).ToString("N0") + " mm");

        // Determinar "F" o "R" de acuerdo al largo anterior y el largo actual
        RopeSpeedFormatter.Instance.RopeDirectionParser(
            ropeLength,
            previousRopeLength,
            ropeIndex: 2
        );

        previousRopeLength = ropeLength;
    }
}
