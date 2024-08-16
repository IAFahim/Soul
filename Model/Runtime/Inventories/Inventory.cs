using System;
using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Model.Runtime.Inventories
{
    [Serializable]
    public abstract class Inventory<T, TV> : ISaveAble where TV : IComparable<TV>, IEquatable<TV>
    {
        [FormerlySerializedAs("items")] [SerializeField] protected UnityDictionary<T, TV> dictionary = new();

        public event Action<T, TV, TV, bool> OnAddedOrIncreased;
        public event Action<T, TV, TV> OnDecreased;
        public event Action<T, TV> OnRemoved;
        public event Action OnInventoryCleared;

        public bool TryGet(T key, out TV amount)
        {
            return dictionary.TryGetValue(key, out amount);
        }

        [Button]
        public void AddOrIncrease(T key, TV amount)
        {
            TV count;
            bool isAdded = false;
            if (dictionary.TryGetValue(key, out TV currentAmount))
            {
                count = AddValues(currentAmount, amount);
            }
            else
            {
                count = amount;
                isAdded = true;
            }

            dictionary[key] = count;
            OnAddedOrIncreased?.Invoke(key, amount, count, isAdded);
        }

        [Button]
        public bool Decrease(T key, TV amount)
        {
            if (!dictionary.TryGetValue(key, out TV currentAmount)) return false;
            if (currentAmount.CompareTo(amount) < 0) return false;
            var count = SubtractValues(currentAmount, amount);

            if (count.Equals(default(TV)))
            {
                Remove(key);
            }
            else
            {
                dictionary[key] = count;
                OnDecreased?.Invoke(key, amount, count);
            }

            return true;
        }

        public bool HasEnough(T key, TV amount)
        {
            return dictionary.TryGetValue(key, out TV currentAmount) && currentAmount.CompareTo(amount) >= 0;
        }

        public bool HasEnough(Pair<T, TV> keyValuePair)
        {
            return dictionary.TryGetValue(keyValuePair.Key, out TV currentAmount) &&
                   currentAmount.CompareTo(keyValuePair.Value) >= 0;
        }
        
        public bool HasEnough(IEnumerable<Pair<T, TV>> keyValues)
        {
            return keyValues.All(HasEnough);
        }


        [Button]
        public bool Remove(T key)
        {
            if (!dictionary.Remove(key, out TV amount)) return false;
            OnRemoved?.Invoke(key, amount);
            return true;
        }

        [Button]
        public void RemoveAll()
        {
            Clear(true);
        }

        public IEnumerable<KeyValuePair<T, TV>> GetAll()
        {
            return dictionary.ToList();
        }

        public bool Has(T key)
        {
            return dictionary.ContainsKey(key);
        }

        public int Count => dictionary.Count;

        protected abstract TV AddValues(TV a, TV b);
        protected abstract TV SubtractValues(TV a, TV b);

        [Button]
        public void Save(string key)
        {
            Data.Save(key, ToList());
        }

        [Button]
        public void Load(string key)
        {
            var list = Data.Load<List<Pair<T, TV>>>(key);
            if (list == null) return;
            Clear(false);
            foreach (var pair in list.Where(pair => pair.Key != null))
            {
                AddOrIncrease(pair.Key, pair.Value);
            }
        }

        public List<Pair<T, TV>> ToList()
        {
            return dictionary.Select(keyValuePair => new Pair<T, TV>(keyValuePair.Key, keyValuePair.Value)).ToList();
        }

        public void Clear(bool invokeEvents)
        {
            if (invokeEvents)
            {
                foreach (var keyValuePair in dictionary)
                {
                    OnRemoved?.Invoke(keyValuePair.Key, keyValuePair.Value);
                }

                OnInventoryCleared?.Invoke();
            }

            dictionary.Clear();
        }
    }
}