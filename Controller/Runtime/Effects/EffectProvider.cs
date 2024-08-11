using Alchemy.Inspector;
using Pancake;
using Soul.Model.Runtime.Effects;
using Soul.Model.Runtime.Modifiers;
using UnityEngine;

namespace Soul.Controller.Runtime.Effects
{
    public class EffectProvider : GameComponent
    {
        public GameComponent target;
        [SerializeReference] public Effect effect;
        public Modifier effectStrengthModifier;
        public float duration = 1;

        [Button]
        public void Apply()
        {
            effect = new FreezeEffect();
            effect.Apply(target.GetComponent<IEffectTarget>(), effectStrengthModifier, duration);
        }
    }
}