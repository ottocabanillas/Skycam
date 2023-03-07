using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SetFloorImage : MonoBehaviour
{
    [SerializeField]
    RawImage floorImage; 

    private string encodedImage;

    // Para crear la textura que luego asignaremos a la variable floorImage
    private Texture2D texture;
    void Start()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(CommonConfigKeys.UPLOADED_IMAGE.ToString())))
        {
            // Recuperamos el string base64 que representa a la imagen guardada.
            encodedImage = PlayerPrefs.GetString(CommonConfigKeys.UPLOADED_IMAGE.ToString());
            // Array de bytes para convertir el string en base64 a arr de bytes
            byte[] bytes = System.Convert.FromBase64String(encodedImage);
            texture = new Texture2D(180,120);
            texture.LoadImage(bytes);
        }
    
        if (floorImage != null)
        {
            // asignar la textura cargada al componente de imagen
            floorImage.texture = texture;
        }
    }
}