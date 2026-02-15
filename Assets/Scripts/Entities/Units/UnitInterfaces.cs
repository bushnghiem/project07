using UnityEngine;

public interface Movable
{
    void Move();
}

public interface Shootable
{
    void Shoot();
}

public interface Unit : Movable, Shootable, Entity
{
    void Item();

    void Initialize(ShipRunData runData);
}