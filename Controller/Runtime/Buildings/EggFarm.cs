using Soul.Controller.Runtime.InfoPanels;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings
{
    public class EggFarm : UnlockUpgradeAbleBuilding ,IInfoPanelReference
    {
        public override string Title { get; }
        public override void OnSelected(RaycastHit selfRayCastHit)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanUpgrade { get; }
        public override bool IsUpgrading { get; }
        public override void Upgrade()
        {
            throw new System.NotImplementedException();
        }

        public override bool CanUnlock { get; }
        public override bool IsUnlocking { get; }
        public override void Unlock()
        {
            throw new System.NotImplementedException();
        }

        public override void Save(string key)
        {
            throw new System.NotImplementedException();
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
        }

        public override void Load(string key)
        {
            throw new System.NotImplementedException();
        }

        public IInfoPanel InfoPanelPrefab { get; }
        public Vector3 InfoPanelWorldPosition { get; }
    }

}