using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DialogBoxController : MonoBehaviour
{
    [SerializeField]
    GameObject dialogPanel;

    [SerializeField]
    GameObject skycam;

    [SerializeField]
    GameObject progressBar;

    [SerializeField]
    Button optionButton;

    [SerializeField]
    TextMeshProUGUI buttonText;

    private Vector3 m_targetPosition = new Vector3(5f, 1f, 2.5f); // Posicion inicial.

    private float m_speed = 0.2f;

    private float m_initialDistance;

    void Start()
    {
        ShowDialog(true);
        progressBar.SetActive(false);
        Slider slider = progressBar.GetComponent<Slider>();
        slider.value = 0;
        optionButton.onClick.AddListener(OnInitButtonClicked);
    }

    void Update()
    {
        // if (Gamepad.current.buttonSouth.isPressed || Input.GetKey(KeyCode.Alpha1))
        // {
        //     ShowDialog(false);
        //     return;
        // }

    }

    private void ShowDialog(bool show)
    {
        dialogPanel.SetActive(show);
    }

    private void OnInitButtonClicked()
    {
        // Comenzar posicionamiento
        Debug.Log("Btn clicked!");
        StartPositioning(); // Comenzar posicionamiento
        StartCoroutine(ActualizarTextoPosicionando()); // Actualizar el texto del boton
        progressBar.SetActive(true); // Hacer visible la barra de progreso
    }

    // Metodo para iniciar la corutina.
    private void StartPositioning()
    {
        // Initialize the initial distance here to ensure it's set before starting the positioning
        m_initialDistance = Vector3.Distance(skycam.transform.position, m_targetPosition);
        StartCoroutine(PositionSkycam());
    }

    // Corutina para mover la Skycam
    IEnumerator PositionSkycam()
    {
        while (Vector3.Distance(skycam.transform.position, m_targetPosition) > 0.01f)
        {
            skycam.transform.position = Vector3.MoveTowards(skycam.transform.position, m_targetPosition, m_speed * Time.deltaTime);
            UpdateProgressBar();
            yield return null; // Wait until the next frame
        }

        // Llegado al punto de inicio
        SetProgressBar(1.0f); // Progress bar al 100%
        buttonText.text = "¡Posicionado!";

        // Esperar dos segundos antes de ocultar el panel de diálogo
        yield return new WaitForSeconds(2);

        // Ocultar el panel de diálogo
        ShowDialog(false);
    }

    // Corutina para actualizar el texto del boton
    private IEnumerator ActualizarTextoPosicionando()
    {
        Slider slider = progressBar.GetComponent<Slider>();

        int contadorPuntos = 0;
        while (slider.value < 1) // Continúa el ciclo mientras la barra de progreso esté activa
        {
            // Actualiza el texto según el número de puntos
            buttonText.text = "Posicionando" + new string('.', contadorPuntos);
            contadorPuntos = (contadorPuntos + 1) % 4; // Reinicia el contador después de 3 para ciclar entre 0 y 3

            yield return new WaitForSeconds(0.5f); // Espera medio segundo antes de actualizar nuevamente
        }
    }

    private void UpdateProgressBar()
    {
        float remainingDistance = Vector3.Distance(skycam.transform.position, m_targetPosition);
        float progress = 1 - (remainingDistance / m_initialDistance); // Calcular el progreso como porcentaje
        SetProgressBar(progress);
    }

    private void SetProgressBar(float value)
    {
        if (progressBar != null)
        {
            Slider slider = progressBar.GetComponent<Slider>();
            if (slider != null)
            {
                slider.value = value;
            }
            if (value == 1)
            {
                progressBar.SetActive(false);

            }
        }
    }
}

