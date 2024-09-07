using System.Collections.Generic;
using Pancake;
using Pancake.Common;

namespace Soul.Model.Runtime.Effects
{
    public interface IEffectConsumer : IComponent
    {
        public float StatMultiplier { get; set; }
        List<IEffect> ActiveEffects { get; }
        float GetEffectMultiplier(StringConstant effectType);
        int HasEffect(StringConstant effectType);
        bool CanApplyEffectOf(StringConstant effectType);
        float ApplyEffect(IEffect effect);
        void RemoveEffect(IEffect effect);
        void RemoveEffects(StringConstant effectType);
        void RemoveAllEffects();
    }
}