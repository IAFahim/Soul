using System;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;

namespace Soul.Model.Runtime.Items
{
    [CreateAssetMenu(fileName = "item", menuName = "Soul/Item/Create Item")]
    [Serializable]
    public class Item : ScriptableObject, ITitle, ISprite
    {
        public string title;
        [TextArea(3, 5)] public string description;
        public Sprite icon;
        
        public string Title => title;
        public Sprite Sprite => icon;

        public override string ToString()
        {
            return title;
        }
        
        public static implicit operator Sprite(Item item) => item.icon;
    }
}