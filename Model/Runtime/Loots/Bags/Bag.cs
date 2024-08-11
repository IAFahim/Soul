using System;
using System.Collections.Generic;
using _Root.Scripts.Model.Runtime.Items;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Model.Runtime.Loots.Bags
{
    [Serializable]
    public class Bag<T> where T : IProbabilityWeight
    {
        public List<ItemProbability<T>> items = new();
        private float[] _cumulativeWeights;
        private float _totalWeight;

        public void Add(T item)
        {
            items.Add(new ItemProbability<T> { loot = item, probability = item.ProbabilityWeight });
            UpdateCumulativeWeights();
        }

        public void Remove(T item)
        {
            items.RemoveAll(x => EqualityComparer<T>.Default.Equals(x.loot, item));
            UpdateCumulativeWeights();
        }

        public void UpdateWeight(T item, float newWeight)
        {
            var bagItem = items.Find(x => EqualityComparer<T>.Default.Equals(x.loot, item));
            if (bagItem == null) return;
            bagItem.probability = newWeight;
            UpdateCumulativeWeights();
        }

        public T Pick()
        {
            if (items.Count == 0) return default;

            var randomPoint = Random.value * _totalWeight;
            var index = Array.BinarySearch(_cumulativeWeights, randomPoint);
            if (index < 0) index = ~index;

            return items[index].loot;
        }

        private void UpdateCumulativeWeights()
        {
            _cumulativeWeights = new float[items.Count];
            _totalWeight = 0;
            for (var i = 0; i < items.Count; i++)
            {
                _totalWeight += items[i].probability;
                _cumulativeWeights[i] = _totalWeight;
            }
        }
    }
}