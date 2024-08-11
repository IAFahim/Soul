﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using _Root.Scripts.Model.Runtime.Items;

namespace _Root.Scripts.Model.Editor.Items
{
    [CustomEditor(typeof(GameItems))]
    public class ItemReferenceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameItems gameItems = (GameItems)target;

            if (GUILayout.Button("Populate Items"))
            {
                PopulateItems(gameItems);
            }
        }

        private void PopulateItems(GameItems gameItems)
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

            gameItems.allItems = items.ToArray();
            EditorUtility.SetDirty(gameItems);
            AssetDatabase.SaveAssets();
        }
    }
}