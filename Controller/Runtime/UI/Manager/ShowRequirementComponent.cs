using Pancake.Pools;
using QuickEye.Utility;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Items;
using Soul.Controller.Runtime.UI.Components;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.UI.Manager
{
    public class ShowRequirementComponent : MonoBehaviour
    {
        public RectTransform spawnRectTransform;
        public ContainerIconRequiredMax requirementPrefab;

        public EventShowItemRequired eventShowItemRequired;
        public PlayerInventoryReference playerInventoryReference;

        public UnityDictionary<Item, ContainerIconRequiredMax> itemRequirementDictionary;

        private void OnEnable()
        {
            itemRequirementDictionary = new UnityDictionary<Item, ContainerIconRequiredMax>();
            eventShowItemRequired.AddListener(ShowRequirement);
        }

        private void OnDisable()
        {
            eventShowItemRequired.RemoveListener(ShowRequirement);
        }

        private void ShowRequirement(Pair<Item, int>[] itemKeyValuePairs)
        {
            if (itemKeyValuePairs == null)
            {
                Clear();
                return;
            }

            foreach (var itemKeyValuePair in itemKeyValuePairs)
            {
                var item = itemKeyValuePair.Key;
                var requiredAmount = itemKeyValuePair.Value;
                playerInventoryReference.inventory.TryGetValue(item, out var has);
                if (itemRequirementDictionary.ContainsKey(itemKeyValuePair.Key))
                {
                    itemRequirementDictionary[itemKeyValuePair.Key].SetValues(item.icon, requiredAmount, has);
                }
                else
                {
                    var newRequirement =
                        requirementPrefab.gameObject.Request<ContainerIconRequiredMax>(spawnRectTransform);
                    newRequirement.Setup(item.icon, requiredAmount, has);
                    itemRequirementDictionary.Add(itemKeyValuePair.Key, newRequirement);
                }
            }
        }

        private void Clear()
        {
            foreach (var itemRequirement in itemRequirementDictionary)
            {
                itemRequirement.Value.gameObject.Return();
            }

            itemRequirementDictionary.Clear();
        }
    }
}