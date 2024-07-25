using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ScoreText;

    private float Score;

    private void OnEnable()
    {
        EventManager.UpdateUIEvent += CallUpdateUIEvent;
    }

    private void OnDisable()
    {
        EventManager.UpdateUIEvent -= CallUpdateUIEvent;
    }

    void Start()
    {
        
    }

    void Update()
    {
        ScoreText.text = Score.ToString();
    }

    private void CallUpdateUIEvent(float ScoreAdd)
    {
        Score += ScoreAdd;
    }
}
