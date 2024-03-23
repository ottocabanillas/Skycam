using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathModel : MonoBehaviour
{
    // Values for Field
    private float heightValue, widthValue, lengthValue;

    // Values for Argos - Skycam
    // Size
    private float heightSkyCam = 500.0f, widthSkyCam = 500.0f, lengthSkyCam = 500.0f;
    // SP Position
    public static float sPX, sPY, sPZ;
    // SP lenght rope
    public float r1sP, r2sP, r3sP, r4sP;

    private float maxSpeed;
    private float currentSpeed;

    // Values from UC (ESP32)
    // Calculate position
    public float pX, pY, pZ;
    // Real length rope
    public float r1Length, r2Length, r3Length, r4Length; 
    
    // Calculated values ​​for UC
    // Velocidad del motor (X) ---> (dX/time)
    public float v1, v2, v3, v4;
    // Direccion de giro
    public string[] motorsDirections = new string[4];

    public Vector3 mt1, mt2, mt3, mt4;

    // Values ​​to calculate
    // Diferencia entre el pto (x, y, z) del real contra el esperado
    private float dX, dY, dZ;
    public float distanceRope;
    // Time (D/V)
    public float time;
    // Diferencia entre el largo real (rXLength) y y esperado (rXsP) [mm]
    private float dR1, dR2, dR3, dR4;

    // Objects used by unity
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
        setMotorsDirections()
        calculateMotorVelocities()

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
        
        // Length Rope
        r1sP = r1sP * 1000.0f;
        r2sP = r2sP * 1000.0f;
        r3sP = r3sP * 1000.0f;
        r4sP = r4sP * 1000.0f;
        
        // Position Box
        pX = r1sP * 1000.0f; 
        pY = r1sP * 1000.0f; 
        pZ = r1sP * 1000.0f;
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
        // v1 = Math.Round((dR1 / time) * 1000, MidpointRounding.AwayFromZero);
        // v2 = Math.Round((dR2 / time) * 1000, MidpointRounding.AwayFromZero);
        // v3 = Math.Round((dR3 / time) * 1000, MidpointRounding.AwayFromZero);
        // v4 = Math.Round((dR4 / time) * 1000, MidpointRounding.AwayFromZero);

        // Velocidades de prueba seteadas en 50 mm/s
        v1 = 50
        v2 = 50
        v3 = 50
        v4 = 50
        return;
    }
    
    // Funcion encargada de establecer las direcciones de cada motor
    // de acuerdo al signo de cada velocidad calculada
    public void setMotorsDirections()
    {
        motorsDirections[0] = v1 >= 0 ? "F" : "R";
        motorsDirections[1] = v2 >= 0 ? "F" : "R";
        motorsDirections[2] = v3 >= 0 ? "F" : "R";
        motorsDirections[3] = v4 >= 0 ? "F" : "R";
        return;

        // Debug Direccion de motores
        // Debug.Log("Dir Motor 1: " + motorsDirections[0]);
        // Debug.Log("Dir Motor 2: " + motorsDirections[1]);
        // Debug.Log("Dir Motor 3: " + motorsDirections[2]);
        // Debug.Log("Dir Motor 4: " + motorsDirections[3]);
    }
}
