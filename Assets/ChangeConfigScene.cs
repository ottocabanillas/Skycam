using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ChangeConfigScene : MonoBehaviour
{
    [SerializeField]
    Button changeValuesButton, continueWithSameConfigButton;

    void Start()
    {
        changeValuesButton.onClick.AddListener(OnChangeValuesButtonClicked);
        continueWithSameConfigButton.onClick.AddListener(OnContinueButtonClicked);   
    }

    private void OnChangeValuesButtonClicked()
    {
        // Seteamos el valor de isUserChangingConfigValues a TRUE para inidicar que estamos por cambiar 
        // los datos de configuracion que tenemos guardados. Para que hacemos esto? Para cambiar el label del titulo, 
        // el texto del boton para cargar una nueva imagen y el label de texto de la imagen previamente seleccionada.
        bool isUserChangingConfigValues = true;
        // Guardo el valor de isUserChangingConfigValues en PlayerPrefs.
        PlayerPrefs.SetString(CommonConfigKeys.IS_USER_CHANGING_CONFIG.ToString(), isUserChangingConfigValues.ToString());

        // Vuelvo a la escena para cambiar los valores de configuracion.
        SceneManager.LoadScene(1);
    }

    private void OnContinueButtonClicked()
    {
        SceneManager.LoadScene(2);
    }
}
