using System;
using System.Collections.Generic;
using _Root.Scripts.Model.Runtime.Modifiers;
using Pancake.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Model.Runtime.Effects
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

        public DelayHandle effectDelayDelayHandle => _effectDelayHandle;

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