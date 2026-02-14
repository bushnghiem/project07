using UnityEngine;

public interface Entity
{
    Vector3 Position { get; }
    bool isDead { get; }
    void Kill();
}
