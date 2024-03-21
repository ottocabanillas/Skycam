using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
public class SkycamController : MonoBehaviour
{
    // Internal Properties
    // -Ejes X y Z
    private float _horizontalInput,
                  _verticalInput;

    // - Limites del Area 
    private float _xPositiveBoundary = 2.6f,
                  _xNegativeBoundary = 0.55f,
                  _zPositiveBoundary = 1.4f,
                  _zNegativeBoundary = 0.4f,
                  _heightLimitMin = 0.5f, //Altura Minima
                  _heightLimitMax = 1.8f; // Altura Maxima


    // - Velocidades segun Eje
    public float _currentSpeed_X = 0,
                  _currentSpeed_Z = 0,
                  _currentSpeed_Y = 0,
                  _acceleration,
                  _speedMax; // V[m/s]

    public string currentHeight,
                  currentSpeed;

    public GameObject dialogPanel;

    private GlobalVariables g_variables;

    private bool isLeftTriggerPressed, // flag trigger izquierdo
                 isRightTriggerPressed, // flag trigger derecho
                 xBoundaryReached, // flag para limite del campo en el eje X (+/- X)
                 yBoundaryReached, // flag para limite del campo en el eje Y (+/- Y)
                 zBoundaryReached; // flag para limite del campo en el eje Z (+/- Z)

    public float smoothTime = 0.3F;
    private Vector3 currentVelocity = Vector3.zero;
    void Start()
    {
        g_variables = GlobalVariables.Instance;
        // Intenta traer el valor de velocidad máxima desde PlayerPrefs. Si no es nulo,
        // se asigna el valor a _speedMax. De lo contrario, se asigna un valor predeterminado de 0.35f.
        _speedMax = g_variables.maxSpeed;

        // Intenta traer el valor de aceleración máxima desde PlayerPrefs. Si no es nulo,
        // se asigna el valor a _acceleration. De lo contrario, se asigna un valor predeterminado de 0.8f.
        //_acceleration = float.TryParse(PlayerPrefs.GetString(CommonConfigKeys.MAX_VELOCITY.ToString()), out float maxAcceleration) ? maxAcceleration : 0.8f;

        // Inicializamos ambos flags en false
        isLeftTriggerPressed = isRightTriggerPressed = false;

        // Inicializamos los flags de limites del campo en false
        xBoundaryReached = yBoundaryReached = zBoundaryReached = false;
    }

    // Functions
    void Update()
    {
        _horizontalInput = IsPanelVisible() ? 0 : Input.GetAxis("Horizontal"); //(Axis +/- X)

        _verticalInput = IsPanelVisible() ? 0 : Input.GetAxis("Vertical"); //(Axis +/- Z)

        float yAxisMovement = 0;

        if (Input.GetKey(KeyCode.Alpha4)) {
            yAxisMovement = 0.5f;
        }
        if (Input.GetKey(KeyCode.Alpha5)) {
            yAxisMovement = -0.5f;
        }
        if (Gamepad.current != null && !IsPanelVisible())
        {
            float leftTriggerValue = Gamepad.current.leftTrigger.ReadValue();
            float rightTriggerValue = Gamepad.current.rightTrigger.ReadValue();

            isLeftTriggerPressed = leftTriggerValue >= 0.001f && !isRightTriggerPressed;
            isRightTriggerPressed = rightTriggerValue >= 0.001f && !isLeftTriggerPressed;

            yAxisMovement = isLeftTriggerPressed ? -leftTriggerValue : isRightTriggerPressed ? rightTriggerValue : 0f;
        }

        // Establecemos las velocidades actuales en los ejes X, Y y Z, teniendo en cuenta los limites del campo. 
        // Si se alcanzo un limite en un eje, la velocidad se establece en cero para ese eje. 
        _currentSpeed_X = xBoundaryReached ? 0 : _speedMax * _horizontalInput; // Velocidad en el eje X
        _currentSpeed_Z = zBoundaryReached ? 0 : _speedMax * _verticalInput; // Velocidad en el eje Z
        _currentSpeed_Y = yBoundaryReached ? 0 : _speedMax * yAxisMovement; // Velocidad en el eje Y

        Vector3 movement = new Vector3(x: _currentSpeed_X, y: _currentSpeed_Y, z: _currentSpeed_Z);

        transform.Translate(movement * Time.deltaTime); //Para mover la Skycam

        // Delimitador de posicion de posicion (eje x - eje z - eje y)
        Vector3 tempPos = transform.position;
        CheckBoundaries(ref tempPos.x, _xNegativeBoundary, _xPositiveBoundary, ref xBoundaryReached, ref _horizontalInput);
        CheckBoundaries(ref tempPos.z, _zNegativeBoundary, _zPositiveBoundary, ref zBoundaryReached, ref _verticalInput);
        CheckBoundaries(ref tempPos.y, _heightLimitMin, _heightLimitMax, ref yBoundaryReached, ref yAxisMovement);
        transform.position = tempPos;

        currentHeight = transform.position.y.ToString("N2");
        // Guardamos la velocidad actual en una variable global
        //g_variables.currentSpeed = Math.Clamp(movement.magnitude, 0.0, 0.35);

        g_variables.currentSpeed = 0.05; // 50 mm/s

        g_variables.spx = transform.position.x; // Posicion en el eje X de la Skycam
        g_variables.spy = transform.position.z; // Posicion en el eje Y de la Skycam
        g_variables.spz = transform.position.y; // Posicion en el eje Z de la Skycam

        // Calcular DX, DY, DZ
        g_variables.calculateDeltaX();
        g_variables.calculateDeltaY();
        g_variables.calculateDeltaZ();

        //Debug.Log("distancia DX: " + g_variables.dX);
        //Debug.Log("distancia DY: " + g_variables.dY);
        //Debug.Log("distancia DZ: " + g_variables.dZ);

        // Calcular D
        g_variables.calculateDistance();
        // Calcular T
        g_variables.calculateTime();
        
        //Debug.Log("distancia D: " + g_variables.d);
        //Debug.Log("Tiempo T: " + g_variables.T);
    }

    void CheckBoundaries(ref float position, float negativeBoundary, float positiveBoundary, ref bool boundaryReached, ref float joystickInput)
    {
        position = Mathf.Clamp(position, negativeBoundary, positiveBoundary);
        // Chequeamos si hemos alcanzado el limite de movimiento (ya sea positivo o negativo) en la direccion correspondiente, y tambien si todavía estamos moviéndonos en esa dirección
        // ya que si no hacemos este chequeo adicional, boundaryReached se hace verdadero y la velocidad del eje correspondiente se establece en cero, 
        // lo que significa que no podemos movernos más en ninguna direccion para el eje correspondiente, y por lo tanto nunca mas cambia el valor de boundaryReached.
        if (position == negativeBoundary && joystickInput < 0 || position == positiveBoundary && joystickInput > 0)
        {
            boundaryReached = true;
        }
        else { boundaryReached = false; }
    }
    public Boolean IsCameraStopped()
    {
        return _currentSpeed_X == 0f && _currentSpeed_Y == 0f && _currentSpeed_Z == 0f;
    }

    public bool IsPanelVisible()
    {
        return dialogPanel.activeSelf;
    }
}
