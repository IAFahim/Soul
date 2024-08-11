using System;
using Pancake.Common;
using UnityEngine;

namespace Soul.Model.Runtime.Effects
{
    [Serializable]
    public abstract class Effect : IEffect
    {
        [SerializeField] private float effectStrength = 1;
        private DelayHandle _effectDelayHandle;
        private IEffectTarget _effectTarget;

        public float EffectStrength
        {
            get => effectStrength;
            protected set => effectStrength = value;
        }

        public abstract EffectType GetEffectType();

        public DelayHandle EffectDelayDelayHandle => _effectDelayHandle;

        public IEffectTarget EffectTarget => _effectTarget;

        public virtual DelayHandle Apply(IEffectTarget target, float strength, float duration)
        {
            _effectTarget = target;
            effectStrength = strength;
            EffectTarget.AddEffect(this);
            return _effectDelayHandle = App.Delay(duration, OnComplete, OnUpdate);
        }

        public abstract void OnComplete();

        public virtual void Cancel(IEffectTarget effectTarget)
        {
            effectTarget.RemoveEffect(this);
        }

        public virtual void OnUpdate(float progressTime)
        {
        }
    }
}