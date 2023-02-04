using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeThreeController : MonoBehaviour
{
    public GameObject cube, rope, pole; // Para asignar Skycam, cuerda y poste desde el inspector
    private Vector3 rightTopVertex, ropeEnd, poleTop;
    private float ropeLength;
    private LineRenderer lineRenderer;
    void Start()
    {
        // Get the top right vertex of the cube
        rightTopVertex = cube.transform.TransformPoint(new Vector3(-0.50f, 0.50f, 0.50f)); // Tenemos que re-nombrar esta variable en los scripts
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
        rightTopVertex = cube.transform.TransformPoint(new Vector3(-0.50f, 0.50f, 0.50f));
        
        Bounds bounds = pole.GetComponent<Renderer>().bounds;
        poleTop = bounds.max;
        
        lineRenderer.SetPosition(0, rightTopVertex);
        lineRenderer.SetPosition(1, poleTop);
        
        // Update the rope length as the cube moves
        ropeLength = Vector3.Distance(rightTopVertex, rope.transform.position);
    }
}
