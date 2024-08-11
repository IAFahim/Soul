using System;
using UnityEngine;

namespace Soul.Model.Runtime.Loots.Bags
{
    [Serializable]
    public class ItemProbability<T>
    {
        public T loot;
        [Range(0, 100)] public float probability = 1;
    }
}