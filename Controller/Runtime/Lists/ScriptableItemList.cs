using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Lists
{
    [CreateAssetMenu(fileName = "newList", menuName = "Soul/List/Item")]
    public class ScriptableItemList : ScriptableList<Item>
    {
    }
}