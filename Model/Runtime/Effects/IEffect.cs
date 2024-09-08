using LitMotion;
using Pancake;

namespace Soul.Model.Runtime.Effects
{
    public interface IEffect
    {
        StringConstant EffectType { get; }
        IEffectConsumer Consumer { get; }
        float Duration { get; }
        public float EffectStrength { get; }
        MotionHandle EffectMotionHandle { get; }
        bool TryApply(IEffectConsumer effectConsumer);
        bool CanApplyTo(IEffectConsumer consumer);
        void Cancel();
    }
}