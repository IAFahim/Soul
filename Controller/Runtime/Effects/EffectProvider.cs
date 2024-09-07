using Alchemy.Inspector;
using Soul.Model.Runtime.Effects;
using Soul.Model.Runtime.Modifiers;
using UnityEngine;

namespace Soul.Controller.Runtime.Effects
{
    public class EffectProvider : MonoBehaviour
    {
        public EffectConsumer consumer;
        [SerializeReference] public IEffect effect;
        public Modifier effectStrengthModifier;
        public float duration = 1;

        [Button]
        public void Apply()
        {
            effect.TryApply(consumer);
        }
    }
}