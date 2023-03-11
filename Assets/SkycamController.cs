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
    private float _xPositiveBoundary = 9.35f,
                  _xNegativeBoundary = 0.65f,
                  _zPositiveBoundary = 4.3f,
                  _zNegativeBoundary = 0.7f,
                  _heightLimitMin = 0.5f, //Altura Minima
                  _heightLimitMax = 4.5f; // Altura Maxima

    // - Velocidades segun Eje
    public float _currentSpeed_X = 0,
                  _currentSpeed_Z = 0,
                  _currentSpeed_Y = 0,
                  _acceleration,
                  _speedMax; // V[m/s]

    public string currentHeight,
                  currentSpeed;

    private bool isLeftTriggerPressed, // flag trigger izquierdo
                 isRightTriggerPressed; // flag trigger derecho
    void Start()
    {
        // Intenta traer el valor de velocidad máxima desde PlayerPrefs. Si no es nulo,
        // se asigna el valor a _speedMax. De lo contrario, se asigna un valor predeterminado de 0.6f.
        _speedMax = float.TryParse(PlayerPrefs.GetString(CommonConfigKeys.MAX_VELOCITY.ToString()), out float maxVelocity) ? maxVelocity : 0.6f;

        // Intenta traer el valor de aceleración máxima desde PlayerPrefs. Si no es nulo,
        // se asigna el valor a _acceleration. De lo contrario, se asigna un valor predeterminado de 0.8f.
        _acceleration = float.TryParse(PlayerPrefs.GetString(CommonConfigKeys.MAX_VELOCITY.ToString()), out float maxAcceleration) ? maxAcceleration : 0.8f;

        // Inicializamos ambos flags en false
        isLeftTriggerPressed = isRightTriggerPressed = false;
    }

    // Functions
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal"); //(Axis +/- X)
        
        _verticalInput = Input.GetAxis("Vertical"); //(Axis +/- Z)
        
        float yAxisMovement = 0;

        if (Gamepad.current != null) 
        {
            float leftTriggerValue = Gamepad.current.leftTrigger.ReadValue();
            float rightTriggerValue = Gamepad.current.rightTrigger.ReadValue();

            isLeftTriggerPressed = leftTriggerValue >= 0.001f && !isRightTriggerPressed;
            isRightTriggerPressed = rightTriggerValue >= 0.001f && !isLeftTriggerPressed;

            yAxisMovement = isLeftTriggerPressed ? -leftTriggerValue : isRightTriggerPressed ? rightTriggerValue : 0f;
        }

        //Velocidad de cada eje
        _currentSpeed_X = _speedMax * _horizontalInput;  // Velocidad en el eje X
        _currentSpeed_Z = _speedMax * _verticalInput; // Velocidad en el eje Z
        _currentSpeed_Y = _speedMax * yAxisMovement; // Velocidad en el eje Y
        Vector3 movement = new Vector3(x: _currentSpeed_X, y: _currentSpeed_Y, z: _currentSpeed_Z);

        transform.Translate(movement * Time.deltaTime); //Para mover la Skycam

        // Delimitador de posicion de posicion (eje x - eje z - eje y)
        Vector3 tempPos = transform.position;
        CheckBoundaries(ref tempPos.x, _xNegativeBoundary, _xPositiveBoundary);
        CheckBoundaries(ref tempPos.z, _zNegativeBoundary, _zPositiveBoundary);
        CheckBoundaries(ref tempPos.y, _heightLimitMin, _heightLimitMax);
        transform.position = tempPos;

        currentHeight = transform.position.y.ToString("N2");
        currentSpeed = movement.magnitude.ToString("N2");
    }

    void CheckBoundaries(ref float position, float negativeBoundary, float positiveBoundary)
    {
        position = Mathf.Clamp(position, negativeBoundary, positiveBoundary);
        //Debug.Log("Position" + position.ToString("N2"));
    }

    void CheckBoundaries1(ref float position, float minBoundary, float maxBoundary)
    {
        if ((position >= maxBoundary) || (position <= minBoundary)) {
            currentSpeed = "0.00";
        }
    }
}
