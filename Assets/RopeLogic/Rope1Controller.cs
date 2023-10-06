using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

    void Start()
    {
        lineRenderer = rope.GetComponent<LineRenderer>();
        ropePole = pole.transform.TransformPoint(new Vector3(0.35f, 1.00f, 0.35f));
        lineRenderer.SetPosition(0, ropePole);

        rightTopVertex = cube.transform.TransformPoint(new Vector3(-0.25f, 0.25f, -0.25f));
        previousRopeLength = Vector3.Distance(rightTopVertex, ropePole);
    }

    void Update()
    {
        // Update the rope's end position to match the left top vertex of the cube
        rightTopVertex = cube.transform.TransformPoint(new Vector3(-0.25f, 0.25f, -0.25f));
        lineRenderer.SetPosition(1, rightTopVertex);

        // Actualizar el largo de la cuerda mientras la Skycam se mueve
        ropeLength = Vector3.Distance(rightTopVertex, ropePole);
        //double realLenght = Math.Pow((4.75 - 0.18), 2) + Math.Pow((1.25 - 6.0), 2) + Math.Pow((2.15 - 0.18), 2);

        //Debug.Log("Largo real: " + Math.Sqrt(realLenght));
        Debug.Log("Largo cuerda 1: " + ropeLength);
        
        // Aca establecemos F o R dependiendo si se debe soltar o contraer cuerda
        // antes de pasarle el largo actual y el largo anterior redondeamos a dos decimales.
        RopeSpeedFormatter.Instance.RopeDirectionParser(
                ropeLength, // reemplazar por realLenght
                previousRopeLength,
                ropeIndex: 0
        );

        // Actualizo el valor del previousRopeLength
        previousRopeLength = ropeLength;
    }
}
