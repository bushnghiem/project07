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
    GameObject GameObject { get; }

    bool IsPlayerControllable { get;}

    int Initiative {  get;}

    void Item();

    void Initialize(ShipRunData runData);

    void StartTurn();

    void EndTurn();
}