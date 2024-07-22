using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumManager : MonoBehaviour
{
    public int Score;
    public int TotalScore;
    public float ScorePerSecond;
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
        TotalScore += (int)(ScorePerSecond * Time.deltaTime);
        UpdateScoreText(TotalScore);
    }

    public void GotScore()
    {
        Score++;
    }

    public void GotTotalScore()
    {
        Debug.Log(TotalScore);
    }
    
    void UpdateScoreText(int Score)
    {
        GetComponent<Text>().text = "Score: " + Score;
    }
    
}
