using System;
using LitMotion;
using Pancake;
using UnityEngine;

namespace Soul.Model.Runtime.Effects
{
    [Serializable]
    public abstract class Effect : IEffect, IDisposable
    {
        [SerializeField] protected StringConstant effectName;
        public float baseDuration;
        private MotionHandle _effectMotionHandle;
        public IEffectConsumer Consumer { get; protected set; }
        public StringConstant EffectName => effectName;

        public MotionHandle EffectMotionHandle
        {
            get => _effectMotionHandle;
            protected set => _effectMotionHandle = value;
        }

        public abstract float EffectStrength { get; }
        public abstract float Duration { get; }
        public bool CanApplyTo(IEffectConsumer effectConsumer) => effectConsumer.CanApplyEffectOf(EffectName);
        public abstract void Apply(IEffectConsumer effectConsumer);

        public virtual bool TryApply(IEffectConsumer effectConsumer)
        {
            bool canApply = CanApplyTo(Consumer = effectConsumer);
            if (canApply) Apply(effectConsumer);
            return canApply;
        }

        public virtual void OnUpdate(float progress)
        {
        }

        public void Cancel()
        {
            Consumer?.RemoveEffect(this);
            EffectMotionHandle.Cancel();
        }

        public abstract void OnComplete();

        public virtual void Dispose() => Cancel();
    }
}