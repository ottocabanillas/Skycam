using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematic
{
    private static InverseKinematic _instance;

    private double L, // Largo del campo
                   H, // Ancho del campo
                   D; // ancho del campo

    // Valores constantes (dimensiones skycam)
    private const double a = 0.5;
    private const double b = 0.5;
    private const double c = 0.5;

    private const double R = 0.3; // Averiguar valor real del radio del motor

    // Propiedad estática para acceder a la instancia
    public static InverseKinematic Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InverseKinematic();
                _instance.Start(); // inicializamos las variables de medidas del campo
            }
            return _instance;
        }
    }

    void Start()
    {
        // Medidas del campo. Tengo que cambiar esto por las medidas del cuarto de Otto
        L = 10.0; // Largo del campo
        H = 6.0;  // Alto del campo
        D = 5.0;  // Ancho del campo
    }

    public Vector4 CalculateAngularVelocities(double x, double y, double z, double L1, double L2, double L3, double L4)
    {
        // Definimos los elementos de la matriz
        double m11 = (x - (a / 2)) / L1;
        double m12 = (y - (b / 2)) / L1;
        double m13 = -(H - (z + (c / 2))) / L1;

        double m21 = (x - (a / 2)) / L2;
        double m22 = -(D - (y + (b / 2))) / L2;
        double m23 = -(H - (z + (c / 2))) / L2;

        double m31 = -(L - (x + (a / 2))) / L3;
        double m32 = -(D - (y + (b / 2))) / L3;
        double m33 = -(H - (z + (c / 2))) / L3;

        double m41 = -(L - (x + (a / 2))) / L4;
        double m42 = (y - (b / 2)) / L4;
        double m43 = -(H - (z + (c / 2))) / L4;

        // Definimos los componentes del vector
        double vX = x;
        double vY = y;
        double vZ = z;

        // Calculamos las velocidades angulares y convertimos a float
        float omega1 = (float)((m11 * vX + m12 * vY + m13 * vZ) / R);
        float omega2 = (float)((m21 * vX + m22 * vY + m23 * vZ) / R);
        float omega3 = (float)((m31 * vX + m32 * vY + m33 * vZ) / R);
        float omega4 = (float)((m41 * vX + m42 * vY + m43 * vZ) / R);

        // Retornamos el resultado como Vector4
        return new Vector4(omega1, omega2, omega3, omega4);
    }
}
