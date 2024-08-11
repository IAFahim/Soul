using System;
using System.Collections.Generic;
using _Root.Scripts.Model.Runtime.Upgrades;
using Alchemy.Inspector;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Upgrades
{
    public class PartsManager : GameComponent
    {
        [SerializeField] private UpgradeParts[] upgradeParts;
        [SerializeField] private Bounds bounds;
        [SerializeField] private List<GameObject> instantiatedParts;

        [Button]
        public void Spawn(int index, bool usePooling)
        {
            instantiatedParts = new(upgradeParts[index].SpawnParts(Transform, usePooling));
        }

        [Button]
        public void Spawn(int index, bool usePooling, BoxCollider boxCollider)
        {
            var parts = upgradeParts[index];
            bounds = parts.bounds;
            instantiatedParts = new(parts.SpawnParts(Transform, usePooling));
            SetBounds(boxCollider);
        }

        [Button]
        public void SpawnExtra(int next, bool usePooling)
        {
            var extraParts = upgradeParts[next - 1].SpawnExtraParts(Transform, upgradeParts[next], usePooling);
            instantiatedParts.AddRange(extraParts);
        }

        [Button]
        public void CaptureChildrenFor(int index)
        {
            upgradeParts[index].StoreAllChildren(Transform);
        }
        
        [Button]
        private void SetBounds(BoxCollider boxCollider)
        {
            boxCollider.center = bounds.center;
            boxCollider.size = bounds.size;
        }

        private void OnDrawGizmosSelected()
        {
            if (bounds.size == Vector3.zero) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.TransformPoint(bounds.center), bounds.size);
        }
    }
}