using System;
using Alchemy.Inspector;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;

namespace Soul.Model.Runtime.Items
{
    [CreateAssetMenu(fileName = "item", menuName = "Soul/Item/Create Item")]
    [Serializable]
    public class Item : ScriptableObject, ITitle, IIcon
    {
        [Title("Item")] public string title;

        [TextArea(3, 5)] public string description;
        public Sprite icon;
        [Range(0, 100)] public float probabilityWeight=1;
        public Sprite Icon => icon;

        public string Title => title;

        public override string ToString()
        {
            return title;
        }

        public static implicit operator Sprite(Item item)
        {
            return item.icon;
        }
    }
}