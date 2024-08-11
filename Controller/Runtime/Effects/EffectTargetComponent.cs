using System.Collections.Generic;
using Pancake;
using Soul.Model.Runtime.Effects;
using UnityEngine;

namespace Soul.Controller.Runtime.Effects
{
    public class EffectTargetComponent : GameComponent, IEffectTarget
    {
        [SerializeReference] private List<IEffect> activeEffects;
        public List<IEffect> ActiveEffects => activeEffects;

        public virtual bool AddEffect(IEffect effect)
        {
            activeEffects.Add(effect);
            return true;
        }

        public bool RemoveEffect(IEffect effect) => activeEffects.Remove(effect);

        public int RemoveEffectAll(EffectType effectType)
        {
            return activeEffects.RemoveAll(effect => effect.GetEffectType() == effectType);
        }

        public float GetEffectMultiplier(EffectType effectType)
        {
            return 1;
        }

        public int ContainsEffect(EffectType effectType)
        {
            return activeEffects.FindAll(effect => effect.GetEffectType() == effectType).Count;
        }
    }
}