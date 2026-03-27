using UnityEngine;
using System.Collections.Generic;

public class EffectController : MonoBehaviour
{
    public List<Effect> effects;

    public void TriggerEffects(Vector3 position)
    {
        Entity entity = GetComponent<Entity>();

        EffectContext context = new EffectContext(
            position,
            gameObject,
            entity
        );

        foreach (var effect in effects)
        {
            effect.Execute(context);
        }
    }
}