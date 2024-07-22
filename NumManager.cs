using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumManager : MonoBehaviour
{
    public int Score;
    public int TotalScore;
    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GotScore();
        TotalScore = Score;
    }

    public void GotScore()
    {
        Score++;
    }

    public void GotTotalScore()
    {
        Debug.Log(TotalScore);
    }
}
