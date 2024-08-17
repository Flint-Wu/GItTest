using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimePlay : MonoBehaviour
{
    public GameObject EndVideo, Endcamera;
    public GameObject ThanksPic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && EndVideo.activeSelf)
        {
            EndVideo.SetActive(false);
            ThanksPic.SetActive(true);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EndVideo.SetActive(true);
        Endcamera.SetActive(true);
    }


}
