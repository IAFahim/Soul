using System;
using Pancake.Common;
using UnityEngine;

namespace Soul.Model.Runtime.Effects
{
    [Serializable]
    public abstract class Effect : IEffect
    {
        [SerializeField] private float effectStrength = 1;

        public IEffectTarget EffectTarget { get; private set; }

        public float EffectStrength
        {
            get => effectStrength;
            protected set => effectStrength = value;
        }

        public abstract EffectType GetEffectType();

        public DelayHandle EffectDelayDelayHandle { get; private set; }

        public virtual DelayHandle Apply(IEffectTarget target, float strength, float duration)
        {
            EffectTarget = target;
            effectStrength = strength;
            EffectTarget.AddEffect(this);
            return EffectDelayDelayHandle = App.Delay(duration, OnComplete, OnUpdate);
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