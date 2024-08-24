using Cysharp.Threading.Tasks;
using Soul.Controller.Runtime.InfoPanels;
using Soul.Model.Runtime.Buildings;
using Soul.Model.Runtime.Levels;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings
{
    // public class EggFarm : UnlockUpgradeAbleBuilding, IInfoPanelReference
    // {
    //     [SerializeField] private LevelInfrastructureInfo levelInfrastructureInfo;
    //     [SerializeField] private InfoPanel infoPanelPrefab;
    //
    //     private readonly bool _loadDataOnEnable = true;
    //
    //     #region Title
    //
    //     public override string Title => levelInfrastructureInfo.Title;
    //
    //     #endregion
    //
    //     public override void OnSelected(RaycastHit selfRayCastHit)
    //     {
    //         Debug.Log("Selected: " + this);
    //     }
    //
    //     private async void Start()
    //     {
    //         if (_loadDataOnEnable) Load(Guid);
    //         await SetUp(level);
    //     }
    //
    //     private async UniTask SetUp(Level currentLevel)
    //     {
    //     }
    //
    //     #region ISaveAble
    //
    //     public override void Save(string key)
    //     {
    //     }
    //
    //     #endregion
    //
    //
    //     public override void Save()
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //
    //     public override void Load(string key)
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //
    //     public override bool CanUpgrade { get; }
    //     public override bool IsUpgrading { get; }
    //
    //     public override void Upgrade()
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //
    //     public override bool CanUnlock { get; }
    //     public override bool IsUnlocking { get; }
    //
    //     public override void Unlock()
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //
    //     public IInfoPanel InfoPanelPrefab => infoPanelPrefab;
    //
    //     public Vector3 InfoPanelWorldPosition =>
    //         transform.TransformPoint(levelInfrastructureInfo.GetInfoPanelPositionOffset(level));
    //
    //     private void OnDrawGizmosSelected()
    //     {
    //         Gizmos.color = Color.white;
    //         Gizmos.DrawWireSphere(InfoPanelWorldPosition, 1f);
    //     }
    // }
}