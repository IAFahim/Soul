using System;
using System.Collections.Generic;
using System.Linq;
using _Root.Scripts.Model.Runtime.Containers;
using _Root.Scripts.Model.Runtime.Interfaces;
using Alchemy.Inspector;
using Pancake.Common;
using QuickEye.Utility;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Inventories
{
    [Serializable]
    public abstract class Inventory<T, TV> : ISaveAble where TV : IComparable<TV>, IEquatable<TV>
    {
        [SerializeField] protected UnityDictionary<T, TV> items = new();

        public event Action<T, TV, TV, bool> OnItemAddedOrIncreased;
        public event Action<T, TV, TV> OnItemDecreased;
        public event Action<T, TV> OnItemRemoved;
        public event Action OnInventoryCleared;

        public bool TryGetItem(T item, out TV amount)
        {
            return items.TryGetValue(item, out amount);
        }

        [Button]
        public void AddOrIncreaseItem(T item, TV amount)
        {
            TV count;
            bool isAdded = false;
            if (items.TryGetValue(item, out TV currentAmount))
            {
                count = AddValues(currentAmount, amount);
            }
            else
            {
                count = amount;
                isAdded = true;
            }

            items[item] = count;
            OnItemAddedOrIncreased?.Invoke(item, amount, count, isAdded);
        }

        [Button]
        public bool DecreaseItem(T item, TV amount)
        {
            if (!items.TryGetValue(item, out TV currentAmount)) return false;
            if (currentAmount.CompareTo(amount) < 0) return false;
            var count = SubtractValues(currentAmount, amount);

            if (count.Equals(default(TV)))
            {
                RemoveItem(item);
            }
            else
            {
                items[item] = count;
                OnItemDecreased?.Invoke(item, amount, count);
            }

            return true;
        }

        [Button]
        public bool RemoveItem(T item)
        {
            if (!items.Remove(item, out TV amount)) return false;
            OnItemRemoved?.Invoke(item, amount);
            return true;
        }

        [Button]
        public void RemoveAllItems()
        {
            Clear(true);
        }

        public IEnumerable<KeyValuePair<T, TV>> GetAllItems()
        {
            return items.ToList();
        }

        public bool HasItem(T item)
        {
            return items.ContainsKey(item);
        }

        public int ItemCount => items.Count;

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
                AddOrIncreaseItem(pair.Key, pair.Value);
            }
        }

        public List<Pair<T, TV>> ToList()
        {
            return items.Select(item => new Pair<T, TV>(item.Key, item.Value)).ToList();
        }

        public void Clear(bool invokeEvents)
        {
            if (invokeEvents)
            {
                foreach (var item in items)
                {
                    OnItemRemoved?.Invoke(item.Key, item.Value);
                }

                OnInventoryCleared?.Invoke();
            }

            items.Clear();
        }
    }
}