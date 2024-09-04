using System;
using Soul.Controller.Runtime.RequiresAndRewards;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.UI.Manager
{
    [Serializable]
    public class SelectData
    {
        public int foundCount;
        [FormerlySerializedAs("title")] public InterfaceFinder<ITitle> titleReference;
        [FormerlySerializedAs("level")] public InterfaceFinder<ILevel> levelReference;
        [FormerlySerializedAs("requirementForUpgrade")] public InterfaceFinder<IRequirementForUpgradeScriptableReference> requirementForUpgradeReference;

        public int Level => levelReference.Value.Level;
        
        public Pair<Currency, int> CurrencyRequirement(int index) =>
            requirementForUpgradeReference.Value.RequirementForUpgrades.GetRequirement(index).currency;

        public Pair<Item, int>[] ItemsRequirement(int index) =>
            requirementForUpgradeReference.Value.RequirementForUpgrades.GetRequirement(index).items;


        public int GetDataFrom(Transform transform)
        {
            foundCount = 0;
            if (titleReference.TryGet(transform, ref titleReference)) foundCount++;
            if (levelReference.TryGet(transform, ref levelReference)) foundCount++;
            if (requirementForUpgradeReference.TryGet(transform, ref requirementForUpgradeReference)) foundCount++;
            return foundCount;
        }
    }
}