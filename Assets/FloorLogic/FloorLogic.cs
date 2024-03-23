using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLogic : MonoBehaviour
{
    public GameObject floorObject;
    private Vector3 scaleChange, positionChange;
    private float scaleX, scaleY = 0.1f, scaleZ;
    private float positionX, positionY, positionZ;

    // Start is called before the first frame update
    void Start()
    {
        scaleX = PlayerPrefs.GetFloat(CommonConfigKeys.LENGTH.ToString());
        scaleZ = PlayerPrefs.GetFloat(CommonConfigKeys.WIDTH.ToString());

        positionX = scaleX / 2.0f;
        positionY = 0.0f; 
        positionZ = scaleZ / 2.0f;

        scaleChange = new Vector3(scaleX, scaleY, scaleZ);
        floorObject.transform.localScale = scaleChange;

        positionChange = new Vector3(positionX, positionY, positionZ);
        floorObject.transform.position = positionChange;

    }

    // Update is called once per frame
    void Update()
    {
        //floorObject.transform.localScale
    }
}
