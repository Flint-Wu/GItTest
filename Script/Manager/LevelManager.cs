using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;

    public LevelName currentLevel;

    public float fadeDuration;
    private bool isFade;

    void Start()
    {

    }

    void Update()
    {

    }


    public void LevelChange(LevelName from, LevelName to)
    {
        StartCoroutine(LevelChanged(from, to));
    }

    private IEnumerator LevelChanged(LevelName from, LevelName to)
    {
        yield return Fade(1);

        yield return Fade(0);
        Debug.Log("¸ü»»½áÊø");
    }


    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;

        fadeCanvasGroup.blocksRaycasts = true;

        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;

        isFade = false;
    }


}
