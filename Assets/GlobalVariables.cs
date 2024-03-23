using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables
{
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

    public double Rx, // Posicion x real
                  Ry, // Posicion z real
                  Rz;  // Posicion y real


    public double sp1, // Largo deseado cuerda 1, en milimetros
                  sp2, // Largo deseado cuerda 2, en milimetros
                  sp3, // Largo deseado cuerda 3, en milimetros
                  sp4;  // Largo deseado cuerda 4, en milimetros
    public double spx, //  Setpoint X, en milimetros
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

    public double currentSpeed; // Velocidad de vuelo. Entre 0 y 350 mm/s
    public float maxSpeed = 0.05f; // Velocidad maxima. 350 mm/s

    public double dX, // diferencia entre x real y x esperado. En milimetros
                 dY, // distaqncia entre y real e y esperado. En milimetros
                 dZ, // distaqncia entre y real e y esperado. En milimetros
                 d, // raiz de la suma del cuadrado de las diferencias dX, dY, dZ. En milimetros
                 T; // cociente entre d y velocidad de vuelo

    public double d1, // Diferencia entre el largo real (R1) y esperado (SP1). En milimetros
                 d2, // Diferencia entre el largo real (R2) y esperado (SP2). En milimetros
                 d3, // Diferencia entre el largo real (R3) y esperado (SP3). En milimetros
                 d4; // Diferencia entre el largo real (R4) y esperado (SP4). En milimetros

    public double v1, // Velocidad motor 1. Obtenida a partir del cociente entre d1 y T
                 v2, // Velocidad motor 2. Obtenida a partir del cociente entre d2 y T
                 v3, // Velocidad motor 3. Obtenida a partir del cociente entre d3 y T
                 v4; // Velocidad motor 4. Obtenida a partir del cociente entre d4 y T

    // Distancia inicial entre Skycam Unity y Skycam real
    public Vector3 initialDistance;

    // Posicion real de la skycam obtenida con el modelo de Cinematica Directa
    public Vector3 realSkycamPosition;
    public bool isSkycamPositioning = false;

    public string[] motorsDirections = new string[4];

    private GlobalVariables()
    {
        a = 0.04; // Longitud de la skycam
        b = 0.04; // ancho de la skycam
        c = 0.2;  // altura skycam

        // Medidas del campo
        L = 3.15; // Largo del campo
        H = 2.28; // Alto del campo
        D = 1.85; // Ancho del campo   
    }

    /* Las siguientes funciones corresponden al paso 3 */
    public void calculateDeltaX()
    {
        // Calcular DX a partir de la diferencia entre Rx y SPx
        if (double.IsNaN(Rx) || double.IsNaN(spx))
        {
            dX = 0;
            return;
        }

        dX = Rx - spx;
        return;
    }

    public void calculateDeltaY()
    {
        // Calcular DY a partir de la diferencia entre Ry y SPy
        if (double.IsNaN(Rz) || double.IsNaN(spz))
        {
            dY = 0;
            return;
        }

        dY = Rz - spz;
        return;
    }

    public void calculateDeltaZ()
    {
        // Calcular Dz a partir de la diferencia entre Rz y SPz
        if (double.IsNaN(Ry) || double.IsNaN(spy))
        {
            dZ = 0;
            return;
        }
        dZ = Ry - spy;
        return;
    }

    public void calculateDistance()
    {
        d = (float)(Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2) + Math.Pow(dZ, 2)));
        return;
    }

    public void calculateTime()
    {
        if (currentSpeed <= 0)
        {
            T = 0.0;
            return;
        }

        T = d / currentSpeed;
        return;
    }

    /* La siguiente funcion corresponde al paso 4 */
    public void CalculateRopesDiff()
    {
        d1 = (double)(R1 / 1000) - sp1;
        d2 = (double)(R2 / 1000) - sp2;
        d3 = (double)(R3 / 1000) - sp3;
        d4 = (double)(R4 / 1000) - sp4;
        //Debug.Log("D1: " + d1);
        //Debug.Log("D2: " + d2);
        //Debug.Log("D3: " + d3);
        //Debug.Log("D4: " + d4);
        return;
    }

    /* La siguiente funcion corresponde al paso 5 */
    public void CalculateMotorVelocities()
    {
        if (T <= 0)
        {
            v1 = 0;
            v2 = 0;
            v3 = 0;
            v4 = 0;
            return;
        }

        // Velocidades de los 4 motores en [mm/s]
        v1 = Math.Round((d1 / T) * 1000, MidpointRounding.AwayFromZero);
        v2 = Math.Round((d2 / T) * 1000, MidpointRounding.AwayFromZero);
        v3 = Math.Round((d3 / T) * 1000, MidpointRounding.AwayFromZero);
        v4 = Math.Round((d4 / T) * 1000, MidpointRounding.AwayFromZero);
        return;
    }

    // Funcion encargada de establecer las direcciones de cada motor
    // de acuerdo al signo de cada velocidad calculada
    
    public void SetMotorsDirections()
    {
        motorsDirections[0] = v1 >= 0 ? "F" : "R";
        motorsDirections[1] = v2 >= 0 ? "F" : "R";
        motorsDirections[2] = v3 >= 0 ? "F" : "R";
        motorsDirections[3] = v4 >= 0 ? "F" : "R";
        return;
    }

}
