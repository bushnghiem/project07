using UnityEngine;

public interface Entity
{
    Vector3 Position { get; }
    bool isDead { get; }
    void Kill();
    void Hurt(DamageInfo damageInfo);
    void Heal(float amount);
}
