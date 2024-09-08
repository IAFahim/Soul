using System;
using System.Collections.Generic;
using Pancake;
using QuickEye.Utility;
using UnityEngine;

namespace Soul.Model.Runtime.Effects
{
    [Serializable]
    public class EffectStrengthMultiplierLookupTable : ScriptableObject
    {
        public UnityDictionary<StringConstant, UnityDictionary<StringConstant, float>> lookUpTable;

        public float GetMultiplier(StringConstant key, StringConstant effectName)
        {
            if (!lookUpTable.TryGetValue(key, out var dict)) return 1;
            return dict.GetValueOrDefault(effectName, 1);
        }
    }
}