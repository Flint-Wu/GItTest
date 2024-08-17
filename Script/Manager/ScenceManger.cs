using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenceManger : MonoBehaviour
{
    // Start is called before the first frame update
    public static ScenceManger instance;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame


}
