using System;
using _Root.Scripts.Model.Runtime.Effects;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Effects
{
    [Serializable]
    public class BleedEffect : Effect
    {
        [SerializeField] private EffectType effectType = EffectType.Bleed;
        [SerializeField] private float damagePerSecond;
        public override EffectType GetEffectType() => effectType;

        public override DelayHandle Apply(IEffectTarget target, float strength, float duration)
        {
            var delayHandle = base.Apply(target, strength, duration);
            target.GetEffectMultiplier(effectType);
            return delayHandle;
        }


        public override void OnComplete()
        {
        }
    }
}