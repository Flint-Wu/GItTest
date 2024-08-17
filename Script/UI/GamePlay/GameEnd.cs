using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BackTik());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BackTik()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(0);
    }
}
