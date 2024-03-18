using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // Instancia única de la clase
    public static GlobalVariables instance;

    public double L, // Largo del campo
                  H, // Ancho del campo
                  D; // ancho del campo

    public double a, b, c;

    public double R1, // Largo real cuerda 1 en milimetros 
                  R2, // Largo real cuerda 2 en milimetros
                  R3, // Largo real cuerda 3 en milimetros
                  R4; // Largo real cuerda 4 en milimetros

    public double Rx, // Posicion x real
                  Ry, // Posicion z real
                  Rz;  // Posicion y real


    public double sp_1, // Largo deseado cuerda 1, en milimetros
                  sp_2, // Largo deseado cuerda 2, en milimetros
                  sp_3, // Largo deseado cuerda 3, en milimetros
                  sp_4;  // Largo deseado cuerda 4, en milimetros
    public double sp_x, //  Setpoint X, en milimetros
                  sp_y, //  Setpoint Y, en milimetros
                  sp_z;  //  Setpoint Z, en milimetros

    public double x_PositiveBoundary, // Limite del campo en el eje X positivo (largo)
                  x_NegativeBoundary, // Limite del campo en el eje X negativo (largo)
                  y_PositiveBoundary, // Limite del campo en el eje Y positivo (alto)
                  y_NegativeBoundary, // Limite del campo en el eje Y negativo (alto)
                  z_PositiveBoundary, // Limite del campo en el eje Z positivo (ancho)
                  z_NegativeBoundary; // Limite del campo en el eje Z negativo (ancho)

    public Vector3 mt1,
                   mt2,
                   mt3,
                   mt4;

    void Awake()
    {
        // Si la instancia no existe, asignar esta instancia
        if (instance == null)
        {
            instance = this;
            // Hacer que el objeto no se destruya al cargar una nueva escena
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // Si ya existe una instancia, destruir este objeto
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        a = 0.15; // Longitud de la skycam
        b = 0.15; // ancho de la skycam
        c = 0.3;  // altura skycam

        // Medidas del campo
        L = 3.15; // Largo del campo
        H = 2.28; // Alto del campo
        D = 1.85; // Ancho del campo   
    }

    // Update is called once per frame
    void Update()
    {

    }
}
