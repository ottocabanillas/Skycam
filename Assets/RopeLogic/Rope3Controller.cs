using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope3Controller : MonoBehaviour
{
    public GameObject cube,
        rope,
        pole;
    public SkycamController skycamController;
    private Vector3 rightTopVertex,
        ropePole,
        poleTop;
    private float ropeLength, previousRopeLength;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = rope.GetComponent<LineRenderer>();
        ropePole = pole.transform.TransformPoint(new Vector3(-0.35f, 1.00f, -0.35f));

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

        //Debug.Log("Largo cuerda 3: " + ropeLength);
        RopeSpeedFormatter.Instance.RopeDirectionParser(
            ropeLength,
            previousRopeLength,
            ropeIndex: 2
        );

        previousRopeLength = ropeLength;
    }
}
