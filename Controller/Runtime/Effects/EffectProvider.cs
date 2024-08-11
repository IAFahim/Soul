using _Root.Scripts.Model.Runtime.Effects;
using _Root.Scripts.Model.Runtime.Modifiers;
using Alchemy.Inspector;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Effects
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