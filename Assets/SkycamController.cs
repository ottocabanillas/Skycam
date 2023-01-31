using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkycamController : MonoBehaviour
{
    private float _horizontalInput, _verticalInput;
    private float _xPositiveBoundary = 4.75f, _xNegativeBoundary = -4.75f, _zPositiveBoundary = 2.25f, _zNegativeBoundary = -2.25f; 
    private float heightLimitMin = 0.5f; // limite de altura minimo
    private float heightLimitMax = 4.5f; // limite de altura maximo
    public float speed = 2f, acceleration = 0.6f, _currentSpeed = 0;
    
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        float yAxisMovement = 0;
        
        if (Gamepad.current.leftTrigger.isPressed)
        {
            // 2f representa la velocidad maxima que nos movemos en el eje-y
            yAxisMovement = -Gamepad.current.leftTrigger.ReadValue() * 2f; // move down
        }

        if (Gamepad.current.rightTrigger.isPressed)
        {
            yAxisMovement = Gamepad.current.rightTrigger.ReadValue() * 2f; // move up
        }

        // Calcula la distancia del analogico respecto de la posicion de reposo
        float totalDistance = Mathf.Abs(_horizontalInput) + Mathf.Abs(_verticalInput) + Mathf.Abs(yAxisMovement);
        
        // Uso totalDistance para controlar la velocidad
        _currentSpeed = Mathf.Clamp(totalDistance, 0, 1) * 1.2f;

        Vector3 movement = new Vector3(_horizontalInput, yAxisMovement, _verticalInput);
        movement.Normalize();
        
        _currentSpeed += acceleration * Time.deltaTime;
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, speed); //Para respetar la velocidad maxima
        transform.Translate(movement * _currentSpeed * Time.deltaTime); //Para mover la Skycam

        // Controlar limites laterales (eje x - eje z)
        Vector3 tempPos = transform.position;
        CheckBoundaries(ref tempPos.x, _xNegativeBoundary, _xPositiveBoundary);
        CheckBoundaries(ref tempPos.z, _zNegativeBoundary, _zPositiveBoundary);
        transform.position = tempPos;

        // Controlar limite de altura (eje y)
        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, heightLimitMin, heightLimitMax);
        transform.position = position;
    }
    
    void CheckBoundaries(ref float position, float negativeBoundary, float positiveBoundary)
    {
        position = Mathf.Clamp(position, negativeBoundary, positiveBoundary);
    }
}
