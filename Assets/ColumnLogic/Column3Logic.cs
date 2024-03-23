using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column3Logic : MonoBehaviour
{
    public GameObject column3Object;
    private Vector3 scaleChange, positionChange;
    private float columnHeightValue, floorWidthValue, floorLengthValue;
    private float scaleX = 0.1f, scaleY, scaleZ = 0.1f;
    private float positionX, positionZ;   
    // Start is called before the first frame update
    void Start()
    {
        columnHeightValue = PlayerPrefs.GetFloat(CommonConfigKeys.HEIGHT.ToString()) / 2.0f;
        floorWidthValue = PlayerPrefs.GetFloat(CommonConfigKeys.WIDTH.ToString());
        floorLengthValue = PlayerPrefs.GetFloat(CommonConfigKeys.LENGTH.ToString());

        positionX = floorLengthValue + 0.035f;
        positionZ = floorWidthValue + 0.035f;

        scaleChange = new Vector3(scaleX, columnHeightValue, scaleZ);
        column3Object.transform.localScale = scaleChange;

        positionChange = new Vector3(positionX, columnHeightValue, positionZ);
        column3Object.transform.position = positionChange;       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
