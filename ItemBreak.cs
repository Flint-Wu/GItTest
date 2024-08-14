using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip breakSound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break()
    {
        AudioSource.PlayClipAtPoint(breakSound,transform.position);
        //Destroy(this.gameObject);
    }
}
