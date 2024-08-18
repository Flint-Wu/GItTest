using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLight : MonoBehaviour
{
    // Start is called before the first frame update
    public Material lightOn;
    public Material lightOff;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightOn()
    {
        this.GetComponent<MeshRenderer>().material = lightOn;
    }

    public void LightOff()
    {
        this.GetComponent<MeshRenderer>().material = lightOff;
    }
}
