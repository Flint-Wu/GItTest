using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterWay : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Got(float Score)
    {
        EventManager.CallUpdateUIEvent(Score);
    }
}
