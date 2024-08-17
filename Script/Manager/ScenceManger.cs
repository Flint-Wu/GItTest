using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenceManger : MonoBehaviour
{
    // Start is called before the first frame update
    public static ScenceManger instance;
    public GameObject MainCamera;
    public GameObject player;
    public GameObject UIcursor;
    public GameObject Camera;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(MainCamera);
        DontDestroyOnLoad(UIcursor);
        DontDestroyOnLoad(Camera);   
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame


}
