using System.Collections.Generic;
using Soul.Model.Runtime.Items;
using UnityEditor;
using UnityEngine;

namespace Soul.Model.Editor.Items
{
    [CustomEditor(typeof(ItemCatalog))]
    public class ItemReferenceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ItemCatalog itemCatalog = (ItemCatalog)target;

            if (GUILayout.Button("Populate Items"))
            {
                PopulateItems(itemCatalog);
            }
        }

        private void PopulateItems(ItemCatalog itemCatalog)
        {
            string[] guids = AssetDatabase.FindAssets("t:Item");
            var items = new List<Item>();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
                if (item != null)
                {
                    items.Add(item);
                }
            }

            itemCatalog.allItems = items.ToArray();
            EditorUtility.SetDirty(itemCatalog);
            AssetDatabase.SaveAssets();
        }
    }
}