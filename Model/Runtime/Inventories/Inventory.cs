using System;
using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using Pancake.Common;
using QuickEye.Utility;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.SaveAndLoad;
using UnityEngine;

namespace Soul.Model.Runtime.Inventories
{
    [Serializable]
    public abstract class Inventory<TKey, TValue> : ISaveAble where TValue : IComparable<TValue>, IEquatable<TValue>
    {
        [SerializeField] protected UnityDictionary<TKey, TValue> items = new();

        public bool RemoveIfZero { get; set; } = true;
        public int Count => items.Count;

        public event Action<InventoryChangeEventArgs<TKey, TValue>> OnItemChanged;
        public event Action OnInventoryCleared;

        [Button]
        public void Save(string key = default)
        {
            key ??= GetDefaultSaveKey();
            Data.Save(key, ToList());
        }

        public bool TryGetValue(TKey key, out TValue amount) => items.TryGetValue(key, out amount);

        [Button]
        public bool AddOrIncrease(TKey key, TValue addAmount, bool saveOnSuccess = true)
        {
            if (items.TryGetValue(key, out var currentAmount))
            {
                var newAmount = AddValues(currentAmount, addAmount);
                items[key] = newAmount;
                OnItemChanged?.Invoke(new InventoryChangeEventArgs<TKey, TValue>(
                    key, newAmount, addAmount, InventoryChangeType.Increased));
            }
            else
            {
                items[key] = addAmount;
                OnItemChanged?.Invoke(new InventoryChangeEventArgs<TKey, TValue>(
                    key, addAmount, addAmount, InventoryChangeType.Added));
            }

            if (saveOnSuccess) Save();

            return true;
        }

        public bool AddOrIncrease(Pair<TKey, TValue> item, bool saveOnSuccess = true)
        {
            return AddOrIncrease(item.Key, item.Value, saveOnSuccess);
        }

        public bool AddOrIncrease(IEnumerable<Pair<TKey, TValue>> requiredItems, bool saveOnSuccess = true)
        {
            var success = requiredItems.All(item => AddOrIncrease(item.Key, item.Value, false));
            if (success && saveOnSuccess) Save();

            return success;
        }

        [Button]
        public bool TryDecrease(TKey key, TValue subtractAmount, bool saveOnSuccess = true)
        {
            if (!items.TryGetValue(key, out var currentAmount)) return false;

            var newAmount = SubtractValues(currentAmount, subtractAmount);
            if (RemoveIfZero && newAmount.CompareTo(default) <= 0)
            {
                items.Remove(key);
                OnItemChanged?.Invoke(new InventoryChangeEventArgs<TKey, TValue>(
                    key, default, subtractAmount, InventoryChangeType.Removed));
            }
            else
            {
                items[key] = newAmount;
                OnItemChanged?.Invoke(new InventoryChangeEventArgs<TKey, TValue>(
                    key, newAmount, subtractAmount, InventoryChangeType.Decreased));
            }

            if (saveOnSuccess) Save();

            return true;
        }

        public bool TryDecrease(Pair<TKey, TValue> item, bool saveOnSuccess = true)
        {
            return TryDecrease(item.Key, item.Value, saveOnSuccess);
        }

        public bool TryDecrease(IEnumerable<Pair<TKey, TValue>> requiredItems, bool saveOnSuccess = true)
        {
            var success = requiredItems.All(item => TryDecrease(item.Key, item.Value, false));
            if (success && saveOnSuccess) Save();

            return success;
        }

        public bool HasEnough(TKey key, TValue amount)
        {
            return items.TryGetValue(key, out var currentAmount) && currentAmount.CompareTo(amount) >= 0;
        }

        public bool HasEnough(Pair<TKey, TValue> requiredItem) => HasEnough(requiredItem.Key, requiredItem.Value);

        public bool HasEnough(IEnumerable<Pair<TKey, TValue>> requiredItems)
        {
            return requiredItems.All(kvp => HasEnough(kvp.Key, kvp.Value));
        }

        [Button]
        public bool Remove(TKey key, bool saveOnSuccess = true)
        {
            if (!items.Remove(key, out var amount)) return false;
            OnItemChanged?.Invoke(new InventoryChangeEventArgs<TKey, TValue>(
                key, default, amount, InventoryChangeType.Removed));

            if (saveOnSuccess) Save();

            return true;
        }

        [Button]
        public void Clear(bool saveOnSuccess = true)
        {
            items.Clear();
            OnInventoryCleared?.Invoke();

            if (saveOnSuccess) Save();
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetAll()
        {
            return items.ToList();
        }

        public List<Pair<TKey, TValue>> ToList()
        {
            return items.Select(pair => new Pair<TKey, TValue>(pair.Key, pair.Value)).ToList();
        }

        public bool Has(TKey key)
        {
            return items.ContainsKey(key);
        }

        protected abstract TValue AddValues(TValue a, TValue b);
        protected abstract TValue SubtractValues(TValue a, TValue b);

        [Button]
        public bool Load(string key = default)
        {
            key ??= GetDefaultSaveKey();
            var loadedItems = Data.Load<List<Pair<TKey, TValue>>>(key);
            if (loadedItems == null) return false;

            items.Clear();
            foreach (var item in loadedItems.Where(item => item.Key != null)) items[item.Key] = item.Value;

            return true;
        }

        protected virtual string GetDefaultSaveKey()
        {
            return $"Inventory_{typeof(TKey).Name}_{typeof(TValue).Name}";
        }
    }

    public enum InventoryChangeType
    {
        Added,
        Increased,
        Removed,
        Decreased
    }

    public struct InventoryChangeEventArgs<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue NewAmount { get; }
        public TValue ChangeAmount { get; }
        public InventoryChangeType ChangeType { get; }

        public InventoryChangeEventArgs(TKey key, TValue newAmount, TValue changeAmount, InventoryChangeType changeType)
        {
            Key = key;
            NewAmount = newAmount;
            ChangeAmount = changeAmount;
            ChangeType = changeType;
        }
    }
}