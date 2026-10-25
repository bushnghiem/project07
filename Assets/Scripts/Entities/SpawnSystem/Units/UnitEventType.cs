using UnityEngine;

public enum UnitEventType
{
    TurnStart,
    ActionResolved,
    TurnEnd,

    Move,
    Shoot,
    Hurt,
    Heal,
    Death,
    ItemUse,
    Shield,

    NotEnoughAP,
    APPreview,
    APClearPreview
}
