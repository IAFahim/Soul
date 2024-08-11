using System;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Model.Runtime.Items
{
    [CreateAssetMenu(fileName = "item", menuName = "Scriptable/Item/Create Item")]
    [Serializable]
    public class Item : ScriptableObject, ITitle
    {
        [FormerlySerializedAs("titleName")] public string title;
        [TextArea(3, 5)] public string description;
        public Sprite icon;
        [Range(0, 100)] [SerializeReference] public float weight = 1;

        public string Title => title;
    }
}