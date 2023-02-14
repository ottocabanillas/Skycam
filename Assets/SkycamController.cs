using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class SkycamController : MonoBehaviour
{
    // Internal Properties
    // -Ejes X y Z
    private float _horizontalInput, 
                  _verticalInput;
    
    // - Limites del Area 
    private float _xPositiveBoundary = 4.50f, 
                  _xNegativeBoundary = -4.50f, 
                  _zPositiveBoundary = 2.0f, 
                  _zNegativeBoundary = -2.0f, 
                  _heightLimitMin = 0.5f, //Altura Minima
                  _heightLimitMax = 4.5f; // Altura Maxima

    // - Velocidades segun Eje
    public float _speedMax = 8f, //V[m/s]
                  _currentSpeed_X = 0,
                  _currentSpeed_Z = 0,
                  _currentSpeed_Y = 0;

    // Functions
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal"); //(Axis +/- X)
        //Debug.Log(_horizontalInput.ToString("N2"));
        _verticalInput = Input.GetAxis("Vertical"); //(Axis +/- Z)
        //Debug.Log(_verticalInput.ToString("N2"));
        float yAxisMovement = 0;
        
        if (Gamepad.current.leftTrigger.isPressed)
        {
            yAxisMovement = -Gamepad.current.leftTrigger.ReadValue(); // move down
        }

        if (Gamepad.current.rightTrigger.isPressed)
        {
            yAxisMovement = Gamepad.current.rightTrigger.ReadValue(); // move up
        }
        
        //Velocidad de cada eje
        _currentSpeed_X = _speedMax * _horizontalInput;  // Velocidad en el eje X
        _currentSpeed_Z = _speedMax * _verticalInput; // Velocidad en el eje Z
        _currentSpeed_Y = _speedMax * yAxisMovement; // Velocidad en el eje Y
        Vector3 movement = new Vector3(x:_currentSpeed_X, y: _currentSpeed_Y, z:_currentSpeed_Z);
        
        transform.Translate(movement * Time.deltaTime); //Para mover la Skycam
        
        // Delimitador de posicion de posicion (eje x - eje z - eje y)
        Vector3 tempPos = transform.position;
        CheckBoundaries(ref tempPos.x, _xNegativeBoundary, _xPositiveBoundary);
        CheckBoundaries(ref tempPos.z, _zNegativeBoundary, _zPositiveBoundary);
        CheckBoundaries(ref tempPos.y, _heightLimitMin, _heightLimitMax);
        transform.position = tempPos;
        
    }
    
    void CheckBoundaries(ref float position, float negativeBoundary, float positiveBoundary)
    {
        position = Mathf.Clamp(position, negativeBoundary, positiveBoundary);
    }
}
