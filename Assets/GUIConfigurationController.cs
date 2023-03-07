using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using static ConfigurationUtilities;
using UnityEngine.UI;
using TMPro;

public class GUIConfigurationController : MonoBehaviour
{
    [SerializeField]
    TMP_InputField maximumVelocityInput, accelerationInput;
    
    [SerializeField]                      
    Button continueButton, uploadImageButton;
    
    [SerializeField]
    Text sceneTitle,
         velocityLabel,
         imageLabel,
         accelerationLabel,
         uploadedImageButtonText,
         uploadButtonText,
         transmissionVelocityLabel;

    [SerializeField]
    TMP_Dropdown baudiosDropdownMenu;

    [SerializeField]
    RawImage imagenPrevia;

    private string maxVelocityValue, accelerationValue, dropdownValue;

    // Flag para determinar si es la primera vez viendo la pantalla de config inicial.
    private bool isUserChangingConfigValues;

    void Awake()
    {
        // Descomentar lineas 38 a 41 si queres borrar lo guardado en PlayerPrefs y empezar con flujo inicial del programa.
        // PlayerPrefs.DeleteKey(CommonConfigKeys.MAX_VELOCITY.ToString());
        // PlayerPrefs.DeleteKey(CommonConfigKeys.ACCELERATION.ToString());
        // PlayerPrefs.DeleteKey(CommonConfigKeys.UPLOADED_IMAGE.ToString());
        // PlayerPrefs.DeleteKey(CommonConfigKeys.IS_USER_CHANGING_CONFIG.ToString());
        // PlayerPrefs.DeleteKey(CommonConfigKeys.IS_USER_CHANGING_CONFIG.ToString());

        // Fue el unico workaround que encontre para que al volver a la pantalla para cambiar la configuracion
        // se pueda mantener el valor del flag isUserChangingConfigValue.
        // La primera vez que se muestre esta escena la variable sera nula y le asignaremos el valor FALSE, ya que es la primera
        // vez que el usuario setea la configuracion inicial del programa. Es decir que no hay config previa guardada. 
        // Luego, la proxima vez que se muestre esta pantalla ya habra un valor guardado en PlayerPrefs, y es el que usaremos
        // para manejar que mostrarle al usuario.
        isUserChangingConfigValues = bool.TryParse(PlayerPrefs.GetString(CommonConfigKeys.IS_USER_CHANGING_CONFIG.ToString()), out bool isUserChangingConfig) ? isUserChangingConfig : false;
        PlayerPrefs.SetString(CommonConfigKeys.IS_USER_CHANGING_CONFIG.ToString(), isUserChangingConfig.ToString());
    }
    void Start()
    {
        // Leemos el valor del flag desde PlayerPrefs
        isUserChangingConfigValues = ConfigurationUtilities.IsUserChangingConfig();

        // El boton 'COMENZAR' debe estar deshabilitado hasta que carguemos todos los valores.
        continueButton.interactable = false;
        continueButton.onClick.AddListener(OnContinueButtonClicked);

        // La ultima conficion !isUserChangingConfigValues es para saber cuando mostrar la pantalla
        // donde preguntamos si quiere cambiar o no la config guardada. Si elige cambiar la config, seteamos
        // isUserChangingConfigValues a true para volver a esta pantalla y entrar en el 'else'
        if (ConfigurationUtilities.AreRequiredConfigValuesSet() && !ConfigurationUtilities.IsUserChangingConfig()) 
        { 
            SceneManager.LoadScene(3);
        }

        else 
        {
            if (isUserChangingConfigValues)
            {
                Debug.Log(PlayerPrefs.GetString(CommonConfigKeys.ACCELERATION.ToString()));
                // Cambiamos los labels del titulo y del boton de cargar imagen del campo.
                sceneTitle.text = "Ingrese los nuevos valores de configuración";
                uploadButtonText.text = "Cargar nueva imagen del campo";

                // Cargamos los valores guardados previamente de 
                // velociad maxima, aceleracion y dropdown menu
                accelerationInput.SetTextWithoutNotify(PlayerPrefs.GetString(CommonConfigKeys.ACCELERATION.ToString()));
                maximumVelocityInput.SetTextWithoutNotify(PlayerPrefs.GetString(CommonConfigKeys.MAX_VELOCITY.ToString()));
                baudiosDropdownMenu.value = PlayerPrefs.GetInt(CommonConfigKeys.BAUDIOS_ARDUINO_OPTION_INDEX.ToString());

                // Mostramos la imagen del campo guardada previamente
                LoadFieldUploadedImage();

                // Seteamos el valor de isUserChangingConfigValues a false
                // debido a que el user actualmente esta cambiando la config previa
                // y la proxima vez que carguemos esta escena tiene que salir la opcion
                // de cambiar los valores guardados o continuar sin cambiar la configuracion.
                SetUserChangingConfigFlagValue(false);

                CheckInputs();
            }
            else 
            {
                sceneTitle.text = "Configuración Inicial";
                uploadButtonText.text = "Cargar imagen del Campo";
                // El label de 'imagen seleccionada' y el placeholder de la imagen que cargue
                // el usuario deben ser invisibles hasta que haya una imagen cargada
                imageLabel.gameObject.SetActive(false);
                imagenPrevia.gameObject.SetActive(false);
            }

            uploadImageButton.onClick.AddListener(OnImageUploadButtonClicked);
        }
    }

    // Función para habilitar/deshabilitar el boton de continuar
    public void CheckInputs()
    {
        // Habilitar el boton COMENZAR solamente si se completaron todos los parametros de la pantalla de config.
        if (maximumVelocityInput.text != "" && baudiosDropdownMenu.value != 0 && accelerationInput.text != ""
            && imagenPrevia.texture != null) 
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }

    private void SaveData()
    {
        // Guardo en una variable el valor del input field de velocidad
        maxVelocityValue = maximumVelocityInput.text;

        // Guardo en una variable el valor del input field de aceleracion
        accelerationValue = accelerationInput.text;

        // Guardo en una variable el valor del input field de velocidad de transmision
        dropdownValue = baudiosDropdownMenu.options[baudiosDropdownMenu.value].text;

        // Guardamos la configuracion inicial en PlayerPrefs
        PlayerPrefs.SetString(CommonConfigKeys.MAX_VELOCITY.ToString(), maxVelocityValue);
        PlayerPrefs.SetString(CommonConfigKeys.BAUDIOS_ARDUINO_STRING.ToString(), dropdownValue);
        PlayerPrefs.SetString(CommonConfigKeys.ACCELERATION.ToString(), accelerationValue);

        // NO CUESTIONES ESTO, SI QUERES REFACTOREALO.
        // Es para saber el indice de la opcion elegida, y sirve para cuando volvemos a cambiar la configuracion.
        PlayerPrefs.SetInt(CommonConfigKeys.BAUDIOS_ARDUINO_OPTION_INDEX.ToString(), baudiosDropdownMenu.value);
    }
    private void OnEnable()
    {
        // Llamar a la función SaveData en los eventos "OnValueChanged" de los InputFields y del Dropdown
        maximumVelocityInput.onValueChanged.AddListener(delegate { SaveData(); CheckInputs(); });
        accelerationInput.onValueChanged.AddListener(delegate { SaveData(); CheckInputs(); });
        baudiosDropdownMenu.onValueChanged.AddListener(delegate { SaveData(); CheckInputs(); });
    }

    private void OnDisable()
    {
        // Remover los listeners de los InputFields y del Dropdown al salir de la escena
        maximumVelocityInput.onValueChanged.RemoveAllListeners();
        accelerationInput.onValueChanged.RemoveAllListeners();
        baudiosDropdownMenu.onValueChanged.RemoveAllListeners();
    }

    private void OnImageUploadButtonClicked()
    {
        // EditorUtility.OpenFilePanel abre la ventana para seleccionar la imagen de la pc
        string ruta = EditorUtility.OpenFilePanel("Selecciona una imagen", "", "jpg");

        if (!string.IsNullOrEmpty(ruta)) {
            Debug.Log("Seleccionaste la imagen: " + ruta);

            // Aquí cargamos la imagen seleccionada en un array de bytes.
            byte[] bytes = System.IO.File.ReadAllBytes(ruta);
            // Creamos una textura 2D
            Texture2D textura = new Texture2D(180, 120);
            // Cargamos los bytes de la imagen en la textura
            textura.LoadImage(bytes);
            // Asignamos la textura al RawImage de la UI
            imagenPrevia.texture = textura;

            // Hacer visible el palceholder de la imagen y su label
            // porque ahora ya tenemos una imagen que mostrar.
            imageLabel.gameObject.SetActive(true);
            imagenPrevia.gameObject.SetActive(true);
            
            // Convertimos lo bytes de la imagen a un string en base64
            string encodedImage = System.Convert.ToBase64String(bytes);

            // Guardamos en playerPrefs la imagen representada en un string base64
            PlayerPrefs.SetString(CommonConfigKeys.UPLOADED_IMAGE.ToString(), encodedImage);

            // Llamamos a la funcion que chequea si se debe habilitar el boton 'COMENZAR'
            CheckInputs();
        }
    }
     private void LoadFieldUploadedImage()
     {
        // Recuperamos la imagen guardada como string y la guardamos en encodedImage
        string encodedImage = PlayerPrefs.GetString(CommonConfigKeys.UPLOADED_IMAGE.ToString());

        if (encodedImage != null)
        {
            // Si el string de la imagen codificada no es nulo, decodificamos la imagen a bytes
            byte[] bytes = System.Convert.FromBase64String(encodedImage);
            
            // Creamos una nueva textura 2D con un tamaño de 180x120
            Texture2D texture = new Texture2D(180, 120);
    
            // Cargamos los bytes de la imagen en la textura 2D
            texture.LoadImage(bytes);
    
            // Asignamos la textura 2D a la variable imagenPrevia
            imagenPrevia.texture = texture;
    
            // Cambiamos el texto de la etiqueta de la imagen a "Imagen cargada previamente"
            imageLabel.text = "Imagen cargada previamente";
    
            // Activamos el objeto imagenPrevia para que sea visible en la pantalla
            imagenPrevia.gameObject.SetActive(true);
        }
        else 
        {
            // Fallo la carga de la imagen
            imageLabel.text = "No hay imagen cargada para el campo";
            // No mostramos el placeholder de la imagen porque no hay imagen guardada
            // o fallo la carga de la misma.
            imagenPrevia.gameObject.SetActive(false);
        }
    }
    private void SetUserChangingConfigFlagValue(bool value)
    {
        isUserChangingConfigValues = value;
        PlayerPrefs.SetString(CommonConfigKeys.IS_USER_CHANGING_CONFIG.ToString(), isUserChangingConfigValues.ToString());    
    }

    private void OnContinueButtonClicked()
    {
        SceneManager.LoadScene(2);
    }
}
