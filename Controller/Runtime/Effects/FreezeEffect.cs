using System;
using _Root.Scripts.Model.Runtime.Effects;
using _Root.Scripts.Model.Runtime.Movements;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Effects
{
    [Serializable]
    public class FreezeEffect : Effect
    {
        [SerializeField] private EffectType effectType = EffectType.Slow;
        public override EffectType GetEffectType() => effectType;

        public override DelayHandle Apply(IEffectTarget target, float strength, float duration)
        {
            var delayHandle = base.Apply(target, strength, duration);
            target.GetEffectMultiplier(effectType);
            Slow(target);
            return delayHandle;
        }

        public override void OnComplete()
        {
            UnSlow(EffectTarget);
            EffectTarget.RemoveEffect(this);
        }

        private void Slow(IEffectTarget target)
        {
            if (target.Transform.TryGetComponent<ISpeedReference>(out var speedReference))
            {
                EffectStrength = Mathf.Min(speedReference.Speed, EffectStrength);
                speedReference.Speed -= EffectStrength;
            }
        }

        private void UnSlow(IEffectTarget target)
        {
            if (target.Transform.TryGetComponent<ISpeedReference>(out var speedReference))
            {
                speedReference.Speed += EffectStrength;
            }
        }
    }
}