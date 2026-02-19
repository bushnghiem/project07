using System;
using UnityEngine;

public static class DeathEvent
{
    public static Action<Entity> OnEntityDeath; // Entity
}


public static class SpawnEvent
{
    public static Action<Unit> OnUnitSpawned;
}