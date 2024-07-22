using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumManager : MonoBehaviour
{
    public int Score;
	
	public int TotalScore;

    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
	
	void GetTotalScore()
	{
		Debug.Log(TotalScore)
	}
}
