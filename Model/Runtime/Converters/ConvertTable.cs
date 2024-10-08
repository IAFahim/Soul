﻿using System;
using QuickEye.Utility;
using UnityEngine;

namespace Soul.Model.Runtime.Converters
{
    [Serializable]
    public class ConvertTable<TInput, TOutput>
    {
        [SerializeField] protected UnityDictionary<TInput, TOutput> conversionDictionary;

        public TOutput Convert(TInput input)
        {
            return conversionDictionary[input];
        }

        public bool TryConvert(TInput input, out TOutput output)
        {
            return conversionDictionary.TryGetValue(input, out output);
        }
    }
}