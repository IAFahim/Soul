using System;
using System.Collections.Generic;
using Soul.Controller.Runtime.Lists;
using Soul.Controller.Runtime.Productions;
using Soul.Controller.Runtime.Upgrades;
using Soul.Model.Runtime.DragAndDrops;
using Soul.Model.Runtime.Items;
using UnityEngine;

namespace Soul.Controller.Runtime.Infrastructures.Farmings
{
    public class FishingPond : FarmingBuilding, IAllowedToDropReference<Item>, IDropAble<Item>
    {
        [SerializeField] private AllowedItemLists allowedItemLists;
        [SerializeField] private BuildingAndProductionRecord buildingAndProductionRecord;
        
        public override int CurrentLevel
        {
            get => buildingAndProductionRecord.level;
            set => buildingAndProductionRecord.level = value;
        }

        public override void OnUnlockUpgradeStart()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUnlockUpgradeComplete(int toLevel)
        {
            throw new System.NotImplementedException();
        }

        public override RecordUpgrade UpgradeRecord { get; set; }
        public IList<Item> ListOfAllowedToDrop => allowedItemLists.CurrentList;
        public bool CanDropNow => !IsLocked && !IsUpgrading;

        public bool OnDrag(Item drop)
        {
            throw new System.NotImplementedException();
        }

        public bool OnDrop(Item dropPackage)
        {
            throw new System.NotImplementedException();
        }

        public void OnDragCancel()
        {
            throw new System.NotImplementedException();
        }
    }
}