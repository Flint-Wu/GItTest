using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreAdd : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = Score.ToString();
    }

    private void CallUpdateUIEvent(float ScoreAdd)
    {
        Score += ScoreAdd;
    }


}
