using System;
using System.Collections.Generic;
using _Root.Scripts.Model.Runtime.Preserves;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Upgrades
{
    [Serializable]
    public class UpgradeParts
    {
        public Bounds bounds;
        [SerializeField] PreservePrefabAndTransform[] parts;

        public PreservePrefabAndTransform[] Parts => parts;

        public GameObject[] SpawnParts(Transform parent, bool usePooling)
        {
            return PopulateArrayBySpawn(parent, usePooling, parts);
        }

        private GameObject[] PopulateArrayBySpawn(Transform parent, bool usePooling,
            PreservePrefabAndTransform[] preservePrefabAndTransforms)
        {
            GameObject[] instantiatedParts = new GameObject[preservePrefabAndTransforms.Length];
            for (int i = 0; i < preservePrefabAndTransforms.Length; i++)
            {
                var part = preservePrefabAndTransforms[i];
                instantiatedParts[i] = usePooling ? part.Request(parent) : part.Instantiate(parent);
            }

            return instantiatedParts;
        }

        public GameObject[] SpawnExtraParts(Transform parent, UpgradeParts nextLevelParts, bool usePooling)
        {
            List<PreservePrefabAndTransform> extraPreservePrefabAndTransforms = new();
            foreach (var next in nextLevelParts.Parts)
            {
                foreach (var part in parts)
                {
                    if (part.prefab == next.prefab) continue;
                    extraPreservePrefabAndTransforms.Add(next);
                }
            }

            return PopulateArrayBySpawn(parent, usePooling, extraPreservePrefabAndTransforms.ToArray());
        }

        public void StoreAllChildren(Transform parent)
        {
            int childCount = parent.childCount;
            parts = new PreservePrefabAndTransform[childCount];
            for (int i = 0; i < childCount; i++)
            {
                var child = parent.GetChild(i);
                var preservePrefabAndTransform = new PreservePrefabAndTransform(child.gameObject, false);
                parts[i] = preservePrefabAndTransform;
            }
        }
        
    }
}