using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectKinematic
{
    private static DirectKinematic _instance;

    public double L, // Largo del campo
                  H, // Ancho del campo
                  D; // ancho del campo
    public double L1, L2, L3, L4; // largo de las cuerdas: 1, 2, 3 y 4 emitidas por ArgosUC
    
    // Valores constantes (dimensiones skycam)
    public double a, b, c;
    
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
                _instance.Start(); // inicializamos los valores constantes
                // Inicializa los valores por defecto aquí o asegúrate de llamar a Initialize antes de usar la instancia
            }
            return _instance;
        }
    }

    private void Start()
    {
        a = 0.15; // Longitud de la skycam (a)
        b = 0.15;  // ancho de la skycam (b)
        c = 0.3; // altura skycam (c)
       
        // Medidas del campo
        L = 3.15; // Largo del campo
        H = 2.28;  // Alto del campo
        D = 1.85;  // Ancho del campo
    }

    public void SetLengthsAndCalculateXYZ(double l1, double l2, double l3, double l4)
    {
        L1 = l1;
        L2 = l2;
        L3 = l3;
        L4 = l4;

        // Debug.Log("valor L1: " + l1);
        // Debug.Log("valor L2: " + l2);
        // Debug.Log("valor L3: " + l3);
        // Debug.Log("valor L4: " + l4);

        // Calculamos X, Y, Z inmediatamente después de inicializar
        _x = CalculateXValue();
        _z = CalculateZValue();
        _y = CalculateYValue(_x, _z);
        // Debug.Log("valor x:" + _x);
        // Debug.Log("valor z:" + _z);
        // Debug.Log("valor y:" + _y);
    }

    public double CalculateXValue()
    {
        double numerador = Math.Pow(L1, 2) - Math.Pow(L4, 2) + Math.Pow(L, 2) - (L*a);
        double denominador = 2 * (L - a);
        
        return RoundNumber(numerador / denominador);
    }

    public double CalculateZValue()
    {
        double numerador = Math.Pow(L4, 2) - Math.Pow(L3, 2) + Math.Pow(D, 2) - (D*b);
        double denominador = 2 * (D-b);
        
        return RoundNumber(numerador / denominador);
    }

    public double CalculateYValue(double x, double z)
    {
        double mitadAlturaSkycam = c / 2; // c/2
        double mitadLargoSkycam = a / 2; // a/2
        double mitadAnchoSkycam = b / 2;  // b/2

        double primerTerminoRaiz = Math.Pow(L1, 2);
        double segundoTerminoRaiz = Math.Pow((x - mitadLargoSkycam), 2);
        double tercerTerminoRaiz = Math.Pow((z - mitadAnchoSkycam), 2);

        double resultado = H - mitadAlturaSkycam - Math.Sqrt(primerTerminoRaiz - segundoTerminoRaiz - tercerTerminoRaiz);
        return RoundNumber(resultado);

    }

    private double RoundNumber(double numberToRound)
    {
        return Math.Round(numberToRound, 2, MidpointRounding.AwayFromZero);
    }
}