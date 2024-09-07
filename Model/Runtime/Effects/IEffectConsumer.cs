using System.Collections.Generic;
using Pancake;
using Pancake.Common;

namespace Soul.Model.Runtime.Effects
{
    public interface IEffectConsumer : IComponent
    {
        public float StatMultiplier { get; set; }
        List<IEffect> ActiveEffects { get; }
        float GetEffectMultiplier(StringConstant effectName);
        int HasEffect(StringConstant effectName);
        bool CanApplyEffectOf(StringConstant effectName);
        float ApplyEffect(IEffect effect);
        void RemoveEffect(IEffect effect);
        void RemoveEffects(StringConstant effectName);
        void RemoveAllEffects();
    }
}