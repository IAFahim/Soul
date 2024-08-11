using System.Collections.Generic;
using Pancake.Common;

namespace _Root.Scripts.Model.Runtime.Effects
{
    public interface IEffectTarget : IComponent
    {
        /// <summary>
        /// List of active effects on the target
        /// </summary>
        List<IEffect> ActiveEffects { get; }

        /// <summary>
        /// Add an effect to the target also add to the list of active effects 
        /// </summary>
        /// <param name="effect"></param>
        /// <returns>true if the effect was added successfully</returns>
        bool AddEffect(IEffect effect);

        /// <summary>
        /// Remove Specific effect from the target
        /// </summary>
        /// <param name="effect"></param>
        /// <returns>true if the effect was removed successfully</returns>
        bool RemoveEffect(IEffect effect);

        /// <summary>
        /// Remove All effects of a specific type from the target
        /// </summary>
        /// <param name="effectType"></param>
        /// <returns>the number of effect removed successfully</returns>
        int RemoveEffectAll(EffectType effectType);

        /// <summary>
        /// Effect multiplier for a specific effect type
        /// </summary>
        /// <example>
        /// Returns how much the effect should be multiplied as Some effect can be more effective on some targets
        /// </example>
        /// <param name="effectType"></param>
        /// <returns> float value of the effect multiplier</returns>
        float GetEffectMultiplier(EffectType effectType);

        /// <summary>
        /// If the target contains a specific effect type
        /// </summary>
        /// <param name="effectType"></param>
        /// <returns>int value of the effect type present</returns>
        int ContainsEffect(EffectType effectType);
    }
}