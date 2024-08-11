using System.Collections.Generic;
using _Root.Scripts.Model.Runtime.Effects;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Effects
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