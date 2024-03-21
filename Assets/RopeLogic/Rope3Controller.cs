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
    private GlobalVariables g_variables; 
    void Start()
    {
        g_variables = GlobalVariables.Instance;
        g_variables.mt3 = new Vector3(3.15f, 1.85f, 2.28f);

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

        Vector3 centroCamara = new Vector3(cube.transform.position.x, cube.transform.position.z, cube.transform.position.y);
        
        // Actualiza el largo de la cuerda 3 mientras la Skycam se mueve
        ropeLength = Vector3.Distance(g_variables.mt3, centroCamara);
        g_variables.sp3 = ropeLength;

        sp3Text.SetText("SP3: " + (g_variables.sp3 * 1000).ToString("N0") + " mm");
    }
}
