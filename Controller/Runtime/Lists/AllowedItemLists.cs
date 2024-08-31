using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Lists
{
    [CreateAssetMenu(fileName = "AllowList", menuName = "Soul/List/Allowed/Item")]
    public class AllowedItemLists : ScriptableObject
    {
        public int currentIndex = 0;
        public ScriptableList<Item>[] allowedLists;
        public ScriptableList<Item> CurrentList => allowedLists[0];

    }
}