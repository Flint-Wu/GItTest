using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event Action<float> UpdateUIEvent;

    public static event Action ButtonPressedEvent;
    public static void CallUpdateUIEvent(float ScoreAdd)
    {
        UpdateUIEvent?.Invoke(ScoreAdd);
    }









    public static void CallButtonPressedEvent()
    {
        ButtonPressedEvent?.Invoke();
    }
}
