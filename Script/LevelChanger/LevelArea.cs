using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelArea : MonoBehaviour
{
    public LevelName beforeLevel;
    public LevelName currentLevel;
    public LevelManager levelManager;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;

    private void Awake()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LevelChanged());
    }

    private IEnumerator LevelChanged()
    {
        yield return Fade(1);

        yield return Fade(0);
        Debug.Log("¸ü»»½áÊø");
    }

    private IEnumerator Fade(float targetAlpha)
    {

        fadeCanvasGroup.blocksRaycasts = true;

        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;

    }
}
