using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables
{
    // Instancia única de la clase
    private static GlobalVariables _instance;

    // Propiedad para acceder a la instancia.
    public static GlobalVariables Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalVariables();
            }
            return _instance;
        }
    }

    public double L, // Largo del campo
                  H, // Ancho del campo
                  D; // ancho del campo

    public double a, b, c;

    public long R1, // Largo real cuerda 1 en milimetros 
                R2, // Largo real cuerda 2 en milimetros
                R3, // Largo real cuerda 3 en milimetros
                R4; // Largo real cuerda 4 en milimetros

    public float Rx, // Posicion x real
                 Ry, // Posicion z real
                 Rz;  // Posicion y real


    public float sp1, // Largo deseado cuerda 1, en milimetros
                 sp2, // Largo deseado cuerda 2, en milimetros
                 sp3, // Largo deseado cuerda 3, en milimetros
                 sp4;  // Largo deseado cuerda 4, en milimetros
    public float spx, //  Setpoint X, en milimetros
                 spy, //  Setpoint Y, en milimetros
                 spz; //  Setpoint Z, en milimetros

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

    public float currentSpeed; // Velocidad de vuelo. Entre 0 y 350 mm/s
    public float maxSpeed = 0.35f; // Velocidad maxima. 350 mm/s

    public float dX, // diferencia entre x real y x esperado. En milimetros
                 dY, // distaqncia entre y real e y esperado. En milimetros
                 dZ, // distaqncia entre y real e y esperado. En milimetros

                 d, // raiz de la suma del cuadrado de las diferencias dX, dY, dZ. En milimetros

                 T; // cociente entre d y velocidad de vuelo

    public float d1, // Diferencia entre el largo real (R1) y esperado (SP1). En milimetros
                 d2, // Diferencia entre el largo real (R2) y esperado (SP2). En milimetros
                 d3, // Diferencia entre el largo real (R3) y esperado (SP3). En milimetros
                 d4; // Diferencia entre el largo real (R4) y esperado (SP4). En milimetros

    public double v1, // Velocidad motor 1. Obtenida a partir del cociente entre d1 y T
                  v2, // Velocidad motor 2. Obtenida a partir del cociente entre d2 y T
                  v3, // Velocidad motor 3. Obtenida a partir del cociente entre d3 y T
                  v4; // Velocidad motor 4. Obtenida a partir del cociente entre d4 y T



    private GlobalVariables()
    {
        a = 0.15; // Longitud de la skycam
        b = 0.15; // ancho de la skycam
        c = 0.3;  // altura skycam

        // Medidas del campo
        L = 3.15; // Largo del campo
        H = 2.28; // Alto del campo
        D = 1.85; // Ancho del campo   

        //maxSpeed = 0.35f;
    }
}
