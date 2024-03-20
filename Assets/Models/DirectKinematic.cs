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

    private GlobalVariables g_variables;

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
        g_variables = GlobalVariables.Instance;

        a = g_variables.a; // Longitud de la skycam (a)
        b = g_variables.b;  // ancho de la skycam (b)
        c = g_variables.c; // altura skycam (c)

        // Medidas del campo
        L = g_variables.L; // Largo del campo
        H = g_variables.H;  // Alto del campo
        D = g_variables.D;  // Ancho del campo
    }

    public void SetLengthsAndCalculateXYZ(double r1, double r2, double r3, double r4)
    {
        L1 = r1;
        L2 = r2;
        L3 = r3;
        L4 = r4;

        // Calculamos X, Y, Z cada vez que recibimos largos de cuerda desde ArgosUC
        g_variables.Rx = CalculateXValue();
        g_variables.Rz = CalculateZValue();
        g_variables.Ry = CalculateYValue(g_variables.Rx, g_variables.Rz);
    }

    public double CalculateXValue()
    {
        double numerador = Math.Pow(L1, 2) - Math.Pow(L4, 2) + Math.Pow(L, 2) - (L * a);
        double denominador = 2 * (L - a);

        return RoundNumber(numerador / denominador);
    }

    public double CalculateZValue()
    {
        double numerador = Math.Pow(L4, 2) - Math.Pow(L3, 2) + Math.Pow(D, 2) - (D * b);
        double denominador = 2 * (D - b);

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