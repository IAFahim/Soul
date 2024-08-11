using System.Collections.Generic;
using QuickEye.Utility;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Effects
{
    public class EffectTracker : ScriptableObject
    {
        public UnityDictionary<Transform, List<Effect>> effectsByTarget = new();
        
        public void AddEffect(Transform transform,Effect effect)
        {
            if (effectsByTarget.TryGetValue(transform, out var effects)) effects.Add(effect);
            else effectsByTarget.Add(transform, new List<Effect> {effect});
        }
        
        public void RemoveEffectAll(Effect effect)
        {
            foreach (var effects in effectsByTarget.Values)
            {
                effects.Remove(effect);
            }
        }
    }
}