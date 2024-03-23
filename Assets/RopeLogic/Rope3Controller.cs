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

    private Vector3 frontLeftTopVertex, ropePole, poleTop;
    private float ropeLength, previousRopeLength;
    private LineRenderer lineRenderer;

    private Vector3 m_t3;
    private GlobalVariables g_variables; 
    void Start()
    {
        g_variables = GlobalVariables.Instance;
        g_variables.mt3 = new Vector3(3.15f, 1.85f, 2.28f);

        lineRenderer = rope.GetComponent<LineRenderer>();
        // ropePole = pole.transform.TransformPoint(new Vector3(-0.35f, 1.00f, -0.35f));
        // lineRenderer.SetPosition(0, ropePole);

        frontLeftTopVertex = cube.transform.TransformPoint(new Vector3(0.25f, 0.25f, 0.25f));
        previousRopeLength = Vector3.Distance(frontLeftTopVertex, ropePole);
    }

    void Update()
    {
        ropePole = pole.transform.TransformPoint(new Vector3(-0.35f, 1.00f, -0.35f));
        lineRenderer.SetPosition(0, ropePole);
        // Update the rope's end position to match the left top vertex of the cube
        frontLeftTopVertex = cube.transform.TransformPoint(new Vector3(0.5f, 0.5f, 0.5f));
        lineRenderer.SetPosition(1, frontLeftTopVertex);

        Vector3 centroCamara = new Vector3(cube.transform.position.x, cube.transform.position.z, cube.transform.position.y);

        ropeLength = Vector3.Distance(frontLeftTopVertex, ropePole);
        g_variables.sp3 = ropeLength;
        // Debug.Log("Cuerda 3: " + ropeLength);
    }
}
