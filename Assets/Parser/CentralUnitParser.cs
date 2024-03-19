using UnityEngine;
using System.Text;
using System;


public class CentralUnitParser
{
    // Unica instancia de CentralUnitParser
    private static CentralUnitParser _instance;

    // Propiedad para acceder a la instancia.
    public static CentralUnitParser Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CentralUnitParser();
            }
            return _instance;
        }
    }

    public long[] m_vmuLengthArr = new long[4]; // Para el largo de cada VMU
    public char[] m_vmuStatArr = new char[4]; // Para el status de cada VMU
    public char m_lastInputChar = '\0'; // Ultimo caracter recibido de Unidad Central
    private static bool m_isSkycamStatusOk = false; // Variable para determinar cuando los 4 VMUs estan OK
    public static bool isSkycamStatusOk // Getter de m_isSkycamStatusOk. Lo usamos en RopeSpeedFormatter
    {
        get { return m_isSkycamStatusOk; }
        set { m_isSkycamStatusOk = value; }
    }

    // Enumeración de los posibles estados.
    private enum State
    {
        ST_INIT, // Estado inicial
        ST_VMU1, // Recibiendo datos VMU 1
        ST_VMU2, // Recibiendo datos VMU 2
        ST_VMU3, // Recibiendo datos VMU 3
        ST_VMU4, // Recibiendo datos VMU 4
        ST_END, // Esperando fin de mensaje '*'
        ST_DEBUG, // Modo debug, '\n' para volver a estado INIT
        ST_ERR // Error
    }

    private State m_currentState; // Variable para mantener el estado actual de CentralUnitParser

    private bool m_isDebugModeOn = false; // Flag modo debug

    private StringBuilder m_numberBuffer; // Buffer para construir el número que representa la longitud de un VMU.

    private long m_vmuLength; // variable temporal para guardar el largo de cada VMU

    // Constructor privado para el patrón Singleton.
    private CentralUnitParser()
    {
        // Inicializa el estado de la CentralUnitParser.
        m_currentState = State.ST_INIT;
        m_numberBuffer = new StringBuilder(20);
    }

    // Método para procesar la entrada y realizar transiciones de estado.
    public void ProcessInput()
    {
        if (m_lastInputChar == '\0')
        {
            // nada que procesar
            return;
        }
        switch (m_currentState)
        {

            case State.ST_INIT:
                {
                    // Cambia a true dependiendo de los estados de las 4 VMUs
                    m_isSkycamStatusOk = false;
                    if (char.IsDigit(m_lastInputChar) || m_lastInputChar == '-')
                    {
                        m_numberBuffer.Append(m_lastInputChar);
                        m_currentState = State.ST_VMU1;
                        break;
                    }
                    if (m_lastInputChar == '#')
                    {
                        m_currentState = State.ST_DEBUG;
                        break;
                    }
                    if (m_lastInputChar == '\n')
                    {
                        // Ignorar saltos de lineas, a menos que estemos en modo DEBUG
                        if (m_isDebugModeOn)
                        {
                            m_currentState = State.ST_INIT;
                            break;
                        }
                        break;
                    }

                    if (m_lastInputChar == '\r')
                    {
                        // Ignorar Carriage Return
                        break;
                    }

                    // Cualquier otro carácter, error
                    m_currentState = State.ST_ERR;
                    m_numberBuffer.Clear(); // Limpiar buffer
                    isSkycamStatusOk = false;
                    break;
                }
            case State.ST_VMU1:
                {
                    if (char.IsDigit(m_lastInputChar) || m_lastInputChar == '-')
                    {
                        m_numberBuffer.Append(m_lastInputChar);
                        break;
                    }
                    if (char.IsLetter(m_lastInputChar))
                    {
                        if (m_numberBuffer.Length == 0)
                        {
                            // error, no recibimos ningun valor numerico
                            m_currentState = State.ST_ERR;
                            break;
                        }
                        // Guardamos el largo y el estado de VMU 1
                        saveVmuLength(m_numberBuffer.ToString(), 0);
                        saveVmuStatus(m_lastInputChar, 0);

                        // Chequeamos si el estado del VMU es Ok
                        checkVmuStatus(m_lastInputChar);

                        // limpiamos el buffer para otras longitudes
                        m_numberBuffer.Clear();

                        // Transicion a ST_VMU2
                        m_currentState = State.ST_VMU2;
                        break;
                    }
                    // Cualquier otro carácter, error
                    m_currentState = State.ST_ERR;
                    break;
                }

            case State.ST_VMU2:
                {
                    if (char.IsDigit(m_lastInputChar) || m_lastInputChar == '-')
                    {
                        m_numberBuffer.Append(m_lastInputChar);
                        break;
                    }
                    if (char.IsLetter(m_lastInputChar))
                    {
                        if (m_numberBuffer.Length == 0)
                        {
                            // error, no recibimos ningun valor numerico
                            m_currentState = State.ST_ERR;
                            break;
                        }
                        // Guardamos el largo y el estado de VMU 2
                        saveVmuLength(m_numberBuffer.ToString(), 1);
                        saveVmuStatus(m_lastInputChar, 1);

                        // Chequeamos si el estado del VMU es Ok
                        checkVmuStatus(m_lastInputChar);

                        // Limpiamos el buffer para otras longitudes
                        m_numberBuffer.Clear();
                        // Transicion a ST_VMU3
                        m_currentState = State.ST_VMU3;
                        break;
                    }
                    // Cualquier otro carácter, error
                    m_currentState = State.ST_ERR;
                    break;
                }

            case State.ST_VMU3:
                {
                    if (char.IsDigit(m_lastInputChar) || m_lastInputChar == '-')
                    {
                        m_numberBuffer.Append(m_lastInputChar);
                        break;
                    }
                    if (char.IsLetter(m_lastInputChar))
                    {
                        if (m_numberBuffer.Length == 0)
                        {
                            // error, no recibimos ningun valor numerico
                            m_currentState = State.ST_ERR;
                            break;
                        }
                        // Guardamos el largo y el estado de VMU 3
                        saveVmuLength(m_numberBuffer.ToString(), 2);
                        saveVmuStatus(m_lastInputChar, 2);
                        // Chequeamos si el estado del VMU es Ok
                        checkVmuStatus(m_lastInputChar);

                        // Limpiamos el buffer para otras longitudes
                        m_numberBuffer.Clear();
                        // Transicion a ST_VMU4
                        m_currentState = State.ST_VMU4;
                        break;
                    }
                    // Cualquier otro carácter, error
                    m_currentState = State.ST_ERR;
                    break;
                }

            case State.ST_VMU4:
                {
                    if (char.IsDigit(m_lastInputChar) || m_lastInputChar == '-')
                    {
                        m_numberBuffer.Append(m_lastInputChar);
                        break;
                    }
                    if (char.IsLetter(m_lastInputChar))
                    {
                        if (m_numberBuffer.Length == 0)
                        {
                            // error, no recibimos ningun valor numerico
                            m_currentState = State.ST_ERR;
                            break;
                        }
                        // Guardamos el largo y estado de VMU 4
                        saveVmuLength(m_numberBuffer.ToString(), 3);
                        saveVmuStatus(m_lastInputChar, 3);
                        // Chequeamos si el estado del VMU es Ok
                        checkVmuStatus(m_lastInputChar);

                        // Limpiamos el buffer
                        m_numberBuffer.Clear();

                        // Transicion a ST_END
                        m_currentState = State.ST_END;
                        break;
                    }

                    // Cualquier otro carácter, error
                    m_currentState = State.ST_ERR;
                    break;
                }

            case State.ST_END:
                {
                    if (m_lastInputChar == '*')
                    {
                        // fin del mensaje
                        m_numberBuffer.Clear();
                        Debug.Log("Central Unit stat OK");
                        Debug.Log("VMU 1: " + m_vmuLengthArr[0] + " VMU 2: " + m_vmuLengthArr[1] + " VMU 3: " + m_vmuLengthArr[1] + " VMU 4: " + m_vmuLengthArr[3]);
                        m_currentState = State.ST_INIT;
                        break;
                    }
                    // Cualquier otro caracter, error
                    m_currentState = State.ST_ERR;
                    break;
                }

            case State.ST_DEBUG:
                {
                    m_isDebugModeOn = true;
                    m_numberBuffer.Append(m_lastInputChar);
                    if (m_lastInputChar == '\n')
                    {
                        m_isDebugModeOn = false;
                        Debug.Log("Unity Debug " + m_numberBuffer.ToString());
                        m_numberBuffer.Clear();
                        m_currentState = State.ST_INIT;
                        break;
                    }
                    break;
                }
            case State.ST_ERR:
                {
                    //Debug.Log("Error parsing");
                    //Debug.Log(m_lastInputChar);
                    m_numberBuffer.Clear();
                    m_isSkycamStatusOk = false;
                    m_currentState = State.ST_INIT;
                    break;
                }
        }
        
        if (m_lastInputChar != '\0')
        {
            m_lastInputChar = '\0';
            return;
        }
    }

    private void saveVmuLength(string vmuLength, int index)
    {
        try
        {
            m_vmuLengthArr[index] = long.Parse(m_numberBuffer.ToString());
            m_numberBuffer.Clear();
            return;
        }
        catch (FormatException)
        {
            Debug.Log("Numero con formato incorrecto");
            m_numberBuffer.Clear();
            m_currentState = State.ST_ERR;
            return;
        }
        catch (OverflowException)
        {
            Debug.Log("Numero con formato incorrecto");
            m_numberBuffer.Clear();
            m_currentState = State.ST_ERR;
            return;
        }
    }

    private void saveVmuStatus(char m_lastInputChar, int index)
    {
        m_vmuStatArr[index] = m_lastInputChar;
        return;
    }

    private void checkVmuStatus(char m_lastInputChar)
    {
        if (m_lastInputChar != 'O')
        {
            m_isSkycamStatusOk = false;
            Debug.Log("Stat not ok: " + m_lastInputChar);
            return;
        }

        m_isSkycamStatusOk = true;
        return;
    }
}