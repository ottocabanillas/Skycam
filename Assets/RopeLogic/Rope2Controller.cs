using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope2Controller : MonoBehaviour
{
    public GameObject cube,
        rope,
        pole; // Para asignar Skycam, cuerda y poste desde el inspector
    public SkycamController skycamController;
    private Vector3 rightTopVertex,
        ropeEnd,
        ropePole,
        poleTop;
    private float ropeLength, previousRopeLength;
    private LineRenderer lineRenderer;

    void Start()
    {
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

        // Update the rope length as the cube moves
        ropeLength = Vector3.Distance(rightTopVertex, ropePole);

        //Parseamos si debemos soltar o contraer la cuerda y en base a esto le asignamos F o R
        RopeSpeedFormatter.Instance.RopeDirectionParser(
            ropeLength,
            previousRopeLength,
            ropeIndex: 1
        );

        // Actualizamos el valor previo
        previousRopeLength = ropeLength;
    }
}
