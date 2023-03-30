using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectKinematic : MonoBehaviour
{
    private static readonly DirectKinematic instance = new DirectKinematic();

    // Valores externos
    private double L, L1, L2, L3, L4, H, W;
    
    // Valores a calcular
    private double x, y, z;
    
    // Valores constantes (dimensiones skycam)
    private double skycamHeight, skycamLenght, skycamWidth;

    private DirectKinematic()
    {
        skycamHeight = skycamLenght = skycamWidth = 0.2;
        L = 10.0;
        H = 6.0;
        W = 5.0;
    }

    public static DirectKinematic Instance
    {
        get { return instance; }
    }

    public void Initialize(double L1, double L2, double L3, double L4)
    {
        this.L1 = L1;
        this.L2 = L2;
        this.L3 = L3;
        this.L4 = L4;
    }

    public double Calculate_X_Value() 
    {
        double numerador = Math.Pow(L, 2) + Math.Pow(L1, 2) - Math.Pow(L4, 2) - (L*skycamHeight);
        double denominador = 2 * (L - skycamHeight);
        
        double temp_x = numerador / denominador;
        
        return temp_x;
    }

    public double Calculate_Z_Value()
    {
        double numerador = Math.Pow(W, 2) + Math.Pow(L4, 2) - Math.Pow(L3, 2) - (W*skycamLenght);
        double denominador = 2 * (W-skycamLenght);
        
        double temp_z = numerador / denominador;
        
        return temp_z;
    }

    public double Calculate_Y_Value(double x, double z)
    {
        double mitadAlturaSkycam = skycamHeight / 2;
        double mitadLargoSkycam = skycamLenght / 2;
        double mitadAnchoSkycam = skycamWidth / 2;

        double primerTerminoRaiz = Math.Pow(L1, 2);
        double segundoTerminoRaiz = Math.Pow((x - mitadAlturaSkycam), 2);
        double tercerTerminoRaiz = Math.Pow((z - mitadAnchoSkycam), 2);

        double temp_y = H + mitadAnchoSkycam - Math.Sqrt(primerTerminoRaiz - segundoTerminoRaiz - tercerTerminoRaiz);
        return temp_y;

    }

    private double RoundNumber(double numberToRound)
    {
        return Math.Round(numberToRound, 2, MidpointRounding.AwayFromZero);
    }
}
