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

    private Vector3 m_skycamPosition = new Vector3();

    // Instancia unica de CentralUnitParser
    private DirectKinematic m_mathModelDk = DirectKinematic.Instance;

    // Instancia unica de RopeSpeedFormatter
    private RopeSpeedFormatter m_ropeSpeedFormatter;

    void Start()
    {
        m_ropeSpeedFormatter = RopeSpeedFormatter.Instance;
        ShowDialog(true);
        progressBar.SetActive(false);
        Slider slider = progressBar.GetComponent<Slider>();
        slider.value = 0;
        optionButton.onClick.AddListener(OnInitSkycamPositioning);
    }

    void Update()
    {
        // if (Gamepad.current.leftShoulder.isPressed || Input.GetKey(KeyCode.Alpha1))
        // {
        //     OnInitSkycamPositioning();
        //     return;
        // }

    }

    private void ShowDialog(bool show)
    {
        dialogPanel.SetActive(show);
    }

    private void OnInitSkycamPositioning()
    {
        // Comenzar posicionamiento
        Debug.Log("Btn clicked!");
        StartPositioning(); // Comenzar posicionamiento
        StartCoroutine(ActualizarTextoPosicionando()); // Actualizar el texto del boton
        progressBar.SetActive(true); // Hacer visible la barra de progreso
    }

    private void StartPositioning()
    {
        // Inicializamos la posicion de la Skycam con los valores X,Y,Z obtenidos a partir del modelo matematico
        m_skycamPosition = new Vector3((float)m_mathModelDk.X, (float)m_mathModelDk.Y, (float)m_mathModelDk.Z);
        //Debug.Log("Skycam initial position: " + m_skycamPosition);
        // La distancia inicial se calcula entre la posicion de la skycam real y la posicion establecida como punto inicial del programa
        m_initialDistance = Vector3.Distance(m_skycamPosition, m_targetPosition);
        // Comenzar el posicionamiento
        StartCoroutine(PositionSkycam());
    }

    // Corutina para el proceso de posicionamiento
    IEnumerator PositionSkycam()
    {
        // Repetir mientras no estemos ubicados en la posicion de inicio (m_targetPosition).
        while (Vector3.Distance(m_skycamPosition, m_targetPosition) > 0.01f)
        {
            // actualizamos la posicion de la skycam en base a los valores calculados X,Y,Z del modelo
            m_skycamPosition = new Vector3((float)m_mathModelDk.X, (float)m_mathModelDk.Y, (float)m_mathModelDk.Z);
            Debug.Log("Skycam position: " + m_skycamPosition);
            UpdateProgressBar();
            yield return null; // Wait until the next frame
        }

        // Llegado al punto de inicio
        SetProgressBar(1.0f); // Progress bar al 100%
        buttonText.text = "¡Posicionado!";
        Debug.Log("Skycam real position: " + m_skycamPosition);
        m_ropeSpeedFormatter.IsSkycamPositioned = true;

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
        float remainingDistance = Vector3.Distance(m_skycamPosition, m_targetPosition);
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

