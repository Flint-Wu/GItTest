using System;
using UnityEngine;

public static class EventCenter
{
    public static event Action Interact;
    public static void CallInteract()
    {
        Interact?.Invoke();
    }
}
