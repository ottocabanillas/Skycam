using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este enum se utiliza como clave de cada uno de los atributos de la configuracion que seteamos
// desde la pantalla de configuración. Estas claves se usan para guardar y recuperar los valores
// de los atributos en PlayerPrefs.
public enum CommonConfigKeys
{
    MAX_VELOCITY,
    ALTO,
    LARGO,
    ANCHO,
    BAUDIOS_ARDUINO_STRING,
    BAUDIOS_ARDUINO_OPTION_INDEX,
    UPLOADED_IMAGE,
    IS_USER_CHANGING_CONFIG
}
