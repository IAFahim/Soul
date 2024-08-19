using System;
using System.Collections.Generic;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Preserves;
using UnityEngine;

namespace Soul.Model.Runtime.Upgrades
{
    [Serializable]
    public class UpgradeParts
    {
        public Bounds bounds;
        [SerializeField] PreservePrefabAndTransform[] parts;

        public PreservePrefabAndTransform[] Parts => parts;

        public Pair<GameObject, bool>[] SpawnParts(Transform parent)
        {
            return PopulateArrayBySpawn(parent, parts);
        }

        private Pair<GameObject, bool>[] PopulateArrayBySpawn(Transform parent,
            PreservePrefabAndTransform[] preservePrefabAndTransforms)
        {
            var instantiatedParts = new Pair<GameObject, bool>[preservePrefabAndTransforms.Length];
            for (int i = 0; i < preservePrefabAndTransforms.Length; i++)
            {
                var part = preservePrefabAndTransforms[i];
                bool usePool = part.PoolOrInstantiate(parent, out GameObject partGameObject);
                instantiatedParts[i] = new Pair<GameObject, bool>(partGameObject, usePool);
            }

            return instantiatedParts;
        }

        public Pair<GameObject, bool>[] SpawnExtraParts(Transform parent, UpgradeParts nextLevelParts, bool usePooling)
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

            return PopulateArrayBySpawn(parent, extraPreservePrefabAndTransforms.ToArray());
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