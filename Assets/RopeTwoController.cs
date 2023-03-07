using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTwoController : MonoBehaviour
{
    public GameObject cube, rope, pole; // Para asignar Skycam, cuerda y poste desde el inspector
    private Vector3 rightTopVertex, ropeEnd, poleTop; 
    private float ropeLength;
    private LineRenderer lineRenderer;
    void Start()
    {
        // Get the top right vertex of the cube
        rightTopVertex = cube.transform.TransformPoint(new Vector3(-0.25f, 0.25f, -0.25f)); // Tenemos que re-nombrar esta variable en los scripts
        ropeEnd = rightTopVertex;
        ropeLength = Vector3.Distance(ropeEnd, rope.transform.position); 
        
		Bounds bounds = pole.GetComponent<Renderer>().bounds;
        poleTop = bounds.max;

        lineRenderer = rope.GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, ropeEnd);  // Set the start position of the rope
        lineRenderer.SetPosition(1, poleTop);  // Set the end position of the rope
    }

    void Update()
    {
        // Update the rope's end position to match the left top vertex of the cube
        rightTopVertex = cube.transform.TransformPoint(new Vector3(-0.25f, 0.25f, -0.25f));

		Bounds bounds = pole.GetComponent<Renderer>().bounds;
        poleTop = bounds.max;
        
        lineRenderer.SetPosition(0, rightTopVertex);
        lineRenderer.SetPosition(1, poleTop);
        
        // Actualizar el largo de la cuerda mientras la Skycam se mueve
        ropeLength = Vector3.Distance(rightTopVertex, rope.transform.position);
    }
}