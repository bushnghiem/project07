using UnityEngine;
using System;

public static class EventBus
{
    public static event Action<UnitEvent> OnEvent;

    public static void Raise(UnitEvent e)
    {
        OnEvent?.Invoke(e);
    }

    public static void Subscribe(Action<UnitEvent> listener)
    {
        OnEvent += listener;
    }

    public static void Unsubscribe(Action<UnitEvent> listener)
    {
        OnEvent -= listener;
    }
}
