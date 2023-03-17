using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope4Controller : MonoBehaviour
{
    public GameObject cube,
        rope,
        pole; // Para asignar Skycam, cuerda y poste desde el inspector
    private Vector3 rightTopVertex,
        ropePole,
        poleTop;
    private float ropeLength;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = rope.GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Update the rope's end position to match the left top vertex of the cube
        rightTopVertex = cube.transform.TransformPoint(new Vector3(0.25f, 0.25f, -0.25f));
        ropePole = pole.transform.TransformPoint(new Vector3(-0.35f, 1.00f, 0.35f));
        Bounds bounds = pole.GetComponent<Renderer>().bounds;
        poleTop = bounds.max;
        lineRenderer.SetPosition(0, rightTopVertex);
        lineRenderer.SetPosition(1, ropePole);

        // Update the rope length as the cube moves
        ropeLength = Vector3.Distance(rightTopVertex, rope.transform.position);
    }
}
