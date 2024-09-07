using System.Collections.Generic;
using Alchemy.Inspector;
using Pancake;
using Pancake.Linq;
using Soul.Model.Runtime.Effects;
using Soul.Model.Runtime.PoolAbles;
using UnityEngine;

namespace Soul.Controller.Runtime.Effects
{
    public abstract class EffectConsumer : PoolAbleComponent, IEffectConsumer
    {
        private void Awake() => ActiveEffects = new List<IEffect>();

        [Min(0)] public float selfEffectMultiplier = 1;

        [SerializeField] protected StringConstant title;
        public abstract void EffectAddSuccess(IEffect effect, float multiplier);
        public abstract void EffectAddFail(IEffect effect);
        public abstract void EffectRemoved(IEffect effect);

        [SerializeField] protected EffectStrengthMultiplierLookupTable effectStrengthMultiplierLookupTable;

        public float StatMultiplier
        {
            get => selfEffectMultiplier;
            set => selfEffectMultiplier = value;
        }

        public List<IEffect> ActiveEffects { get; private set; }

        public float GetEffectMultiplier(StringConstant effectType)
        {
            return effectStrengthMultiplierLookupTable.GetMultiplier(title, effectType) * selfEffectMultiplier;
        }

        public int HasEffect(StringConstant effectType)
        {
            return ActiveEffects.Count(effect => effect.EffectType == effectType);
        }

        public bool CanApplyEffectOf(StringConstant effectType) =>
            !Mathf.Approximately(GetEffectMultiplier(effectType), 0);

        public float ApplyEffect(IEffect effect)
        {
            var multiplier = GetEffectMultiplier(effect.EffectType);
            if (Mathf.Approximately(multiplier, 0))
            {
                EffectAddFail(effect);
                return 0;
            }

            ActiveEffects.Add(effect);
            EffectAddSuccess(effect, multiplier);
            return multiplier;
        }

        public void RemoveEffect(IEffect effect)
        {
            ActiveEffects.Remove(effect);
            EffectRemoved(effect);
        }


        public void RemoveEffects(StringConstant effectType)
        {
            for (var i = ActiveEffects.Count - 1; i >= 0; i--)
            {
                if (ActiveEffects[i].EffectType == effectType) ActiveEffects.RemoveAt(i);
            }
        }

        public void RemoveAllEffects()
        {
            ActiveEffects.ForEach(effect => effect.Cancel());
            ActiveEffects.Clear();
        }


        public override void OnReturn() => RemoveAllEffects();
    }
}