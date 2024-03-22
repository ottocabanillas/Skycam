using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Column1Logic : MonoBehaviour
{
    public GameObject column1Object;
    private Vector3 scaleChange, positionChange;
     private float columnHeightValue;
    private float scaleX = 0.1f, scaleY, scaleZ = 0.1f;
    private float positionX, positionY, positionZ;    
    // Start is called before the first frame update
    void Start()
    {
        columnHeightValue = PlayerPrefs.GetFloat(CommonConfigKeys.HEIGHT.ToString()) / 2.0f;

        positionX = 0.05f;
        positionY = columnHeightValue; 
        positionZ = 0.05f;

        scaleChange = new Vector3(scaleX, columnHeightValue, scaleZ);
        column1Object.transform.localScale = scaleChange;

        positionChange = new Vector3(positionX, columnHeightValue, positionZ);
        column1Object.transform.position = positionChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
