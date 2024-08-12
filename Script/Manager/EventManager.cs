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

    public static event Action InteractEvent;
    public static void CallInteractEvent()
    {
        InteractEvent?.Invoke();
    }

    public static event Action<TextAsset, DialogueEntry> DialogueBeginEvent;
    public static void CallDialogutBeginEvent(TextAsset text, DialogueEntry dialogueEntry)
    {
        DialogueBeginEvent?.Invoke(text,dialogueEntry);
    }

    public static event Action DialogueFinishEvent;
    public static void CallDialogutFinishEvent()
    {
        DialogueFinishEvent?.Invoke();
    }
}
