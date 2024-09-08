using Soul.Model.Runtime.Effects;
using UnityEngine;

namespace Soul.Controller.Runtime.Effects
{
    public class CharacterEffectConsumer : EffectConsumer
    {
        public override void EffectAddSuccess(IEffect effect, float multiplier)
        {
            Debug.Log("Effect added successfully to " + effect.Consumer);
        }

        public override void EffectAddFail(IEffect effect)
        {
            Debug.Log("Effect failed to add to " + effect.Consumer);
        }

        public override void EffectRemoved(IEffect effect)
        {
            Debug.Log("Effect removed from " + effect.Consumer);
        }
    }
}