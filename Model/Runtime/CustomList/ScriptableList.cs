using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;

namespace Soul.Model.Runtime.CustomList
{
    [Serializable]
    public class ScriptableList<T> : ScriptableObject, IList<T>, ITitle
    {
        [SerializeField] private string title;
        [SerializeField] protected List<T> list = new();
        private readonly HashSet<T> _hashSet = new();

        public bool IsEmpty => list.Count == 0;

        public List<T> List => list;

        private void OnEnable()
        {
            foreach (var item in list) _hashSet.Add(item);
        }

        public void Add(T item)
        {
            if (_hashSet.Contains(item)) return;
            list.Add(item);
            _hashSet.Add(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            if (!_hashSet.Contains(item)) return false;
            list.Remove(item);
            _hashSet.Remove(item);
            return true;
        }

        public void Clear()
        {
            list.Clear();
            _hashSet.Clear();
        }

        public bool Contains(T item)
        {
            return _hashSet.Contains(item);
        }

        public int Count => list.Count;
        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            var item = list[index];
            list.RemoveAt(index);
            _hashSet.Remove(item);
        }

        public T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string Title => title;

        public void Remove(T item)
        {
            if (!_hashSet.Contains(item)) return;
            list.Remove(item);
            _hashSet.Remove(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            var collection = items.ToArray();
            if (collection.Length == 0) return;

            list.AddRange(collection);
            _hashSet.UnionWith(collection);
        }

        public bool TryAddRange(IEnumerable<T> items)
        {
            if (items == null) return false;

            var uniqueItems = items.Where(item => !_hashSet.Contains(item)).ToList();
            if (uniqueItems.Count > 0)
            {
                AddRange(uniqueItems);
                return true;
            }

            return false;
        }
    }
}