using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectKinematic : MonoBehaviour
{

    // Valores externos
    private double L, L1, L2, L3, L4, H, D;
    // Valores a calcular
    private double x, y, z;
    // Valores constantes (dimensiones skycam)
    private double skycamHeight, skycamLenght, skycamWidth;

    // Start is called before the first frame update
    void Start()
    {
        L = 100;
        L1 = 36.24;
        L2 = 42.57;
        L3 = 76.18;
        L4 = 72.84;
        H = 20;
        D = 50;
        skycamHeight = skycamLenght = skycamWidth = 0.2;

        x = Calculate_X_Value();
        z = Calculate_Z_Value();
        y = Calculate_Y_Value(x, z);

        Debug.Log("MODELO DE CINEMATICA DIRECTA");
        Debug.Log("Valor calculado de X: " + x);
        Debug.Log("Valor calculado de Z: " + z);
        Debug.Log("Valor calculado de Y: " + y);  
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private double Calculate_X_Value() 
    {
        double numerador = Math.Pow(L, 2) + Math.Pow(L1, 2) - Math.Pow(L4, 2) - (L*skycamHeight);
        double denominador = 2 * (L - skycamHeight);
        
        double temp_x = numerador / denominador;
        
        return RoundNumber(temp_x);
    }

    private double Calculate_Z_Value()
    {
        double numerador = Math.Pow(D, 2) + Math.Pow(L4, 2) - Math.Pow(L3, 2) - (D*skycamLenght);
        double denominador = 2 * (D-skycamLenght);
        
        double temp_z = numerador / denominador;
        
        return RoundNumber(temp_z);
    }

    private double Calculate_Y_Value(double x, double z)
    {
        double mitadAlturaSkycam = skycamHeight / 2;
        double mitadLargoSkycam = skycamLenght / 2;
        double mitadAnchoSkycam = skycamWidth / 2;

        double primerTerminoRaiz = Math.Pow(L1, 2);
        double segundoTerminoRaiz = Math.Pow((x - mitadAlturaSkycam), 2);
        double tercerTerminoRaiz = Math.Pow((z - mitadAnchoSkycam), 2);

        double temp_y = H + RoundNumber(mitadAnchoSkycam) - RoundNumber(Math.Sqrt(primerTerminoRaiz - segundoTerminoRaiz - tercerTerminoRaiz));
        
        return RoundNumber(temp_y);

    }

    private double RoundNumber(double numberToRound)
    {
        return Math.Round(numberToRound, 2, MidpointRounding.AwayFromZero);
    }
}
