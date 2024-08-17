using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenceDoor : MonoBehaviour
{
    // Start is called before the first frame update
    //public bool isOpen = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            other.transform.position = new Vector3(-46,0,2);
        }

    }
}
