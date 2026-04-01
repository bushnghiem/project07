using UnityEngine;
using System;

public static class EventBus
{
    public static Action<UnitEvent> OnEvent;

    public static void Raise(UnitEvent e)
    {
        OnEvent?.Invoke(e);
    }
}
