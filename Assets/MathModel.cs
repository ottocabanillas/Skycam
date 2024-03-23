using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathModel : MonoBehaviour
{
    // Values for Argos
    private float heightValue, widthValue, lengthValue;
    private float heightSkyCam = 1, widthSkyCam = 1, lengthSkyCam = 1;
    public float pX, pY, pZ;
    public static float sPX, sPY, sPZ;
    public float r1Length, r2Length, r3Length, r4Length; 
    public float r1sP, r2sP, r3sP, r4sP;

    private float maxSpeed;
    private float currentSpeed;

    // Diferencia entre el pto (x, y, z) del real contro el esperado
    private float dX, dY, dZ;

    public float distanceRope;

    // Time (D/V)
    public float time;

    // Diferencia entre el largo real (rXLength) y y esperado (rXsP) [mm]
    private float dR1, dR2, dR3, dR4;

    // Velocidad del motor (X) ---> (dX/time)
    public float v1, v2, v3, v4;

    public GameObject skyCamArgos, rope1, rope2, rope3, rope4;

    // Propiedad para acceder a la instancia.
    public static MathModel _instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MathModel();
            }
            return _instance;
        }
    }

    // void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    //     else if (instance != this)
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    void Start()
    {
        setupValues();

    }

    // Update is called once per frame
    void Update()
    {
        calculateMotorVelocities()
        setMotorsDirections()

    }

    void setupValues()
    {
        // Convert to mm 
        // Area de trabajo 
        heightValue = PlayerPrefs.GetFloat(CommonConfigKeys.HEIGHT.ToString()) * 1000.0f;
        widthValue = PlayerPrefs.GetFloat(CommonConfigKeys.WIDTH.ToString()) * 1000.0f;
        lengthValue = PlayerPrefs.GetFloat(CommonConfigKeys.LENGTH.ToString()) * 1000.0f;

        //Debug Valores en [mm]
        Debug.Log(heightValue);
        Debug.Log(widthValue);
        Debug.Log(lengthValue);

        // Convert to mm
        r1sP, 
        r2sP, 
        r3sP, 
        r4sP = r4sP * 1000.0f;


    }

    public void calculateMotorVelocities()
    {
        if (time <= 0)
        {
            v1 = 0;
            v2 = 0;
            v3 = 0;
            v4 = 0;
            return;
        }

        // Velocidades de los 4 motores en [mm/s]
        v1 = Math.Round((dR1 / time) * 1000, MidpointRounding.AwayFromZero);
        v2 = Math.Round((dR1 / time) * 1000, MidpointRounding.AwayFromZero);
        v3 = Math.Round((dR1 / time) * 1000, MidpointRounding.AwayFromZero);
        v4 = Math.Round((dR1 / time) * 1000, MidpointRounding.AwayFromZero);
        return;
    }

    public void setMotorsDirections()
    {
        motorsDirections[0] = v1 >= 0 ? "F" : "R";
        motorsDirections[1] = v2 >= 0 ? "F" : "R";
        motorsDirections[2] = v3 >= 0 ? "F" : "R";
        motorsDirections[3] = v4 >= 0 ? "F" : "R";
        return;
    }
}
