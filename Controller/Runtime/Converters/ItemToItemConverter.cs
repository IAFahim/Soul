using System;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Converters;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Converters
{
    [CreateAssetMenu(fileName = "ItemToItemConverterOverTime", menuName = "Soul/Converter/ItemToItemConverterOverTime")]
    [Serializable]
    public class ItemToItemConverter : ScriptableObject
    {
        public ConvertTable<Item, Pair<Item, float>>[] convertTables;
        public int currentIndex;

        public ConvertTable<Item, Pair<Item, float>> CurrentTable => convertTables[currentIndex];

        public void Set(int index)
        {
            currentIndex = index;
        }
        public bool TryConvert(Item input, out Pair<Item, float> output) => CurrentTable.TryConvert(input, out output);
    }
}