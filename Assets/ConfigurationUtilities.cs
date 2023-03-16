using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationUtilities
{
    public static bool IsUserChangingConfig()
    {
        // valor predeterminado para cuando no hay valor guardado para el flag
        // esto es para la primera vez que se corra el programa desde cero.
        bool defaultValue = false;
        string isUserChangigConfig = PlayerPrefs.GetString(CommonConfigKeys.IS_USER_CHANGING_CONFIG.ToString());
        return string.IsNullOrEmpty(isUserChangigConfig) ? defaultValue : bool.Parse(isUserChangigConfig);
    }

    public static bool AreRequiredConfigValuesSet()
    {
        return !string.IsNullOrEmpty(PlayerPrefs.GetString(CommonConfigKeys.MAX_VELOCITY.ToString())) &&
               PlayerPrefs.GetFloat(CommonConfigKeys.ALTO.ToString()) != 0.0f &&
               PlayerPrefs.GetFloat(CommonConfigKeys.LARGO.ToString()) != 0.0f &&
               PlayerPrefs.GetFloat(CommonConfigKeys.ANCHO.ToString()) != 0.0f &&
               !string.IsNullOrEmpty(PlayerPrefs.GetString(CommonConfigKeys.BAUDIOS_ARDUINO_STRING.ToString())) &&
               !string.IsNullOrEmpty(PlayerPrefs.GetString(CommonConfigKeys.UPLOADED_IMAGE.ToString()));
    }
}
