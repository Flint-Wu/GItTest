using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (Input.GetKeyDown(KeyCode.Space) && EndVideo.activeSelf)
        {
            EndVideo.SetActive(false);
            ThanksPic.SetActive(true);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EndVideo != null)
            EndVideo.SetActive(true);
        if (Endcamera != null)
            Endcamera.SetActive(true);
        if (EndVideo != null)
            EndVideo.transform.root.gameObject.SetActive(true);
    }


}
