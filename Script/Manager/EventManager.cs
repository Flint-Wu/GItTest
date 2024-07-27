using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event Action<float> UpdateUIEvent;
    public static void CallUpdateUIEvent(float ScoreAdd)
    {
        UpdateUIEvent?.Invoke(ScoreAdd);
    }


    public static event Action<GameObject> ItemSave;
    public static void CallItemSave(GameObject gameObject)
    {
        ItemSave?.Invoke(gameObject);
    }

    public static event Action ButtonPressedEvent;
    public static void CallButtonPressedEvent()
    {
        Debug.Log("ButtonPressedEvent");
        ButtonPressedEvent?.Invoke();
    }

}
