using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumManager : MonoBehaviour
{
    public int Score;

    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GotScore();
    }

    public void GotScore()
    {
        Score++;
    }
}
