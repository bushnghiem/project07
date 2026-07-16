using UnityEngine;

public interface Entity : ICameraTarget
{
    Vector3 Position { get; }
    bool isDead { get; }
    ActionContext ActionContext { get; }
    UnitBase Instigator { get; }
    string DisplayName { get; }
    void Kill();
    void Hurt(DamageInfo damageInfo);
    void Heal(float amount);
}
