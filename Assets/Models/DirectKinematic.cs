using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectKinematic
{
    private static DirectKinematic _instance;

    // Valores externos
    public double L, H, W; // largo, alto y ancho del campo
    public double L1, L2, L3, L4; // largo de las cuerdas: 1, 2, 3 y 4
    
    // Valores constantes (dimensiones skycam)
    public double skycamLength, skycamWidth, skycamHeight;
    
    // Valores calculados para X, Y, Z
    private double _x, _y, _z;

    // Propiedades públicas para obtener X, Y, Z
    public double X { get { return _x; } }
    public double Y { get { return _y; } }
    public double Z { get { return _z; } }

    // Propiedad estática para acceder a la instancia
    public static DirectKinematic Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DirectKinematic();
                // Inicializa los valores por defecto aquí o asegúrate de llamar a Initialize antes de usar la instancia
            }
            return _instance;
        }
    }

    private void Start()
    {
        // Ejemplo de inicialización, puede variar según tu configuración
        skycamLength = 0.2; // Longitud de la skycam (c)
        skycamWidth = 0.2;  // ancho de la skycam (b)
        skycamHeight = 0.2; // altura skycam (a)
        L = 10.0; // Largo del campo
        H = 6.0;  // Altura
        W = 5.0;  // Ancho del campo
    }

    public void Initialize(double l1, double l2, double l3, double l4)
    {
        L1 = l1;
        L2 = l2;
        L3 = l3;
        L4 = l4;

        Debug.Log("valor L1: " + l1);
        Debug.Log("valor L2: " + l2);
        Debug.Log("valor L3: " + l3);
        Debug.Log("valor L4: " + l4);

        // Calculamos X, Y, Z inmediatamente después de inicializar
        _x = CalculateXValue();
        _z = CalculateZValue();
        _y = CalculateYValue(_x, _z);
        Debug.Log("valor x:" + _x);
        Debug.Log("valor z:" + _z);
        Debug.Log("valor y:" + _y);
    }

    public double CalculateXValue()
    {
        double numerador = Math.Pow(L1, 2) - Math.Pow(L4, 2) - (L*skycamHeight);
        double denominador = 2 * (L - skycamHeight);
        
        return numerador / denominador;
    }

    public double CalculateZValue()
    {
        double numerador = Math.Pow(L4, 2) - Math.Pow(L3, 2) + Math.Pow(W, 2) - (W*skycamLength);
        double denominador = 2 * (W-skycamLength);
        
        return numerador / denominador;
    }

    public double CalculateYValue(double x, double z)
    {
        double mitadAlturaSkycam = skycamHeight / 2; // c/2
        double mitadLargoSkycam = skycamLength / 2; // a/2
        double mitadAnchoSkycam = skycamWidth / 2;  // b/2

        double primerTerminoRaiz = Math.Pow(L1, 2);
        double segundoTerminoRaiz = Math.Pow((x - mitadAlturaSkycam), 2);
        double tercerTerminoRaiz = Math.Pow((z - mitadAnchoSkycam), 2);

        return H - mitadAnchoSkycam - Math.Sqrt(primerTerminoRaiz - segundoTerminoRaiz - tercerTerminoRaiz);

    }

    private double RoundNumber(double numberToRound)
    {
        return Math.Round(numberToRound, 2, MidpointRounding.AwayFromZero);
    }
}