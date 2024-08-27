using System.Collections.Generic;
using Alchemy.Inspector;
using Pancake;
using Pancake.Pools;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Extensions;
using Soul.Model.Runtime.Upgrades;
using UnityEngine;

namespace Soul.Controller.Runtime.Upgrades
{
    public class PartsManager : GameComponent
    {
        [SerializeField] private UpgradeParts[] upgradeParts;
        [SerializeField] private Bounds bounds;
        [SerializeField] private List<Pair<GameObject, bool>> instantiatedParts;

        private UpgradeParts GetUpgradeParts(int index) => upgradeParts[Mathf.Clamp(index, 0, upgradeParts.Length - 1)];
        
        [Button]
        public void Spawn(int index)
        {
            instantiatedParts = new(GetUpgradeParts(index).SpawnParts(Transform));
        }

        [Button]
        public void Spawn(int index, BoxCollider boxCollider)
        {
            var parts = GetUpgradeParts(index);
            bounds = parts.bounds;
            instantiatedParts = new(parts.SpawnParts(Transform));
            SetBounds(boxCollider);
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

        [Button]
        public void ClearInstantiatedParts()
        {
            foreach (var part in instantiatedParts)
            {
                if (part.Value) part.Key.Return();
                else part.Key.SafeDestroy();
            }

            instantiatedParts.Clear();
        }

        private void OnDrawGizmosSelected()
        {
            if (bounds.size == Vector3.zero) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.TransformPoint(bounds.center), bounds.size);
        }
    }
}