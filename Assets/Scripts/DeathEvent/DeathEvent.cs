using System;
using UnityEngine;

public static class DeathEvent
{
    public static Action<Vector3, GameObject> OnEntityDeath; // Death Position, Entity
}
