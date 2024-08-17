using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoCtrl : MonoBehaviour
{
    public GameObject video,ThanksImg;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VideoEnd()
    {
        video.SetActive(false);
        ThanksImg.SetActive(true);
    }
}
