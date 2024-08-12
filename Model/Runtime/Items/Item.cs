using System;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Model.Runtime.Items
{
    [CreateAssetMenu(fileName = "item", menuName = "Soul/Item/Create Item")]
    [Serializable]
    public class Item : ScriptableObject, ITitle
    {
        public string title;
        [TextArea(3, 5)] public string description;
        public Sprite icon;
        

        public string Title => title;
    }
}