using QuickEye.Utility;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Items
{
    [CreateAssetMenu(fileName = ".crop", menuName = "Scriptable/Item/Create Crop")]
    public class Crop : Item, IWeight
    {
        public UnityTimeSpan growTime;
        public float Weight => weight;
    }
}