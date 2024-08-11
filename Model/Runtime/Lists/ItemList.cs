using _Root.Scripts.Model.Runtime.CustomList;
using _Root.Scripts.Model.Runtime.Items;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Lists
{
    [CreateAssetMenu(fileName = "newList", menuName = "Scriptable/ItemList/Create New")]
    public class ItemList: ScriptableList<Item>
    {
        
    }
}