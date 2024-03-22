using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column4Logic : MonoBehaviour
{
    public GameObject column4Object;
    private Vector3 scaleChange, positionChange;
    private float columnHeightValue, floorWidthValue, floorLengthValue;
    private float scaleX = 0.1f, scaleY, scaleZ = 0.1f;
    private float positionX, positionY, positionZ;    
    // Start is called before the first frame update
    void Start()
    {
        columnHeightValue = PlayerPrefs.GetFloat(CommonConfigKeys.HEIGHT.ToString()) / 2.0f;
        floorWidthValue = PlayerPrefs.GetFloat(CommonConfigKeys.WIDTH.ToString());
        floorLengthValue = PlayerPrefs.GetFloat(CommonConfigKeys.LENGTH.ToString());

        positionX = floorLengthValue - 0.05f;
        positionZ = 0.05f;

        scaleChange = new Vector3(scaleX, columnHeightValue, scaleZ);
        column4Object.transform.localScale = scaleChange;

        positionChange = new Vector3(positionX, columnHeightValue, positionZ);
        column4Object.transform.position = positionChange;         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
