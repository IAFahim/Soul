using LitMotion;
using Pancake;

namespace Soul.Model.Runtime.Effects
{
    public interface IEffect
    {
        StringConstant EffectName { get; }
        IEffectConsumer Consumer { get; }
        bool TryApply(IEffectConsumer effectConsumer);
        public float EffectStrength { get; }
        MotionHandle EffectMotionHandle { get; }
        float Duration { get; }
        bool CanApplyTo(IEffectConsumer consumer);
        void OnUpdate(float progress);
        void Cancel();
    }
}