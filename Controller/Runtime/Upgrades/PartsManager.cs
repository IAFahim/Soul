using System.Collections.Generic;
using Alchemy.Inspector;
using Pancake;
using Soul.Model.Runtime.Upgrades;
using UnityEngine;

namespace Soul.Controller.Runtime.Upgrades
{
    public class PartsManager : GameComponent
    {
        [SerializeField] private UpgradeParts[] upgradeParts;
        [SerializeField] private Bounds bounds;
        [SerializeField] private List<GameObject> instantiatedParts;
        [SerializeField] private int usePooling;
        [Button]
        public void Spawn(int index)
        {
            instantiatedParts = new(upgradeParts[index].SpawnParts(Transform, usePooling>-1));
        }

        [Button]
        public void Spawn(int index, BoxCollider boxCollider)
        {
            var parts = upgradeParts[index];
            bounds = parts.bounds;
            instantiatedParts = new(parts.SpawnParts(Transform, usePooling>-1));
            SetBounds(boxCollider);
        }

        [Button]
        public void SpawnExtra(int next)
        {
            var extraParts = upgradeParts[next - 1].SpawnExtraParts(Transform, upgradeParts[next], usePooling>-1);
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