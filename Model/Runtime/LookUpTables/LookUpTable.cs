using System;
using QuickEye.Utility;
using UnityEngine;

namespace Soul.Model.Runtime.LookUpTables
{
    [Serializable]
    public class LookUpTable<T, TV>
    {
        [SerializeField] private UnityDictionary<T, TV> lookUpTable;

        public bool TryGet(T key, out TV value) => lookUpTable.TryGetValue(key, out value);
    }
}