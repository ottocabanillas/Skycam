using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    public GameObject poste1;
    
    // Start is called before the first frame update
    void Start()
    {
        poste1.transform.localScale = new Vector3(poste1.transform.localScale.x, 5.00f, poste1.transform.localScale.z);
        poste1.transform.position = new Vector3 (poste1.transform.position.x, 5.00f, poste1.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
