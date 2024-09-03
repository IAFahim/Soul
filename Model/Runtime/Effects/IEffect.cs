using Pancake.Common;

namespace Soul.Model.Runtime.Effects
{
    public interface IEffect
    {
        public float EffectStrength { get; }
        DelayHandle EffectDelayDelayHandle { get; }
        public EffectType GetEffectType();
        DelayHandle Apply(IEffectTarget target, float strength, float duration);
        void OnComplete();
        void Cancel(IEffectTarget effectTarget);
        void OnUpdate(float progressTime);
    }
}