using System.Collections;
using Alchemy.Inspector;
using Links.Runtime;
using LitMotion;
using Soul.Controller.Runtime.Indicators;
using Soul.Controller.Runtime.InfoPanels;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.SelectableComponents;
using Soul.Model.Runtime.Buildings;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.RequiredAndRewards.Rewards;
using Soul.Model.Runtime.Tweens;
using Soul.Model.Runtime.Tweens.Scriptable;
using UnityEngine;

namespace Soul.Presenter.Runtime.Infrastructures.Farmings
{
    public class WoodChop : BaseSelectableComponent, IRewardClaim
    {
        [Title("WoodChop")] [SerializeField] protected LevelInfrastructureInfo infrastructureInfo;
        public InfoPanel infoPanel;
        public PlayerFarmReference playerFarm;

        public IInfoPanel InfoPanelPrefab => infoPanel;
        public float duration = 3f;
        public Item wood;
        private WaitForSecondsRealtime waitForDuration;

        public Vector3 InfoPanelWorldPosition =>
            transform.TransformPoint(infrastructureInfo.GetInfoPanelPositionOffset(0));

        private Coroutine coroutine;

        public override string Title => infrastructureInfo.Title;
        private IPlayStopAble _sawMill;

        #region Selected Animation

        [SerializeField] protected TweenSettingCurveScriptableObject<Vector3> selectTweenSetting;
        protected MotionHandle SelectTweenMotionHandle;


        public IndicatorProgressCapacity indicatorProgressCapacity;

        private void OnEnable()
        {
            _sawMill = GetComponent<IPlayStopAble>();
            waitForDuration = new WaitForSecondsRealtime(duration);
            indicatorProgressCapacity.Change(0, 1);
            indicatorProgressCapacity.Change(wood);
            
        }

        protected void PlayDualSquishAndStretch()
        {
            if (SelectTweenMotionHandle.IsActive()) SelectTweenMotionHandle.Cancel();
            SelectTweenMotionHandle = transform.TweenScale(selectTweenSetting);
        }

        public override void OnSelected(RaycastHit selfRayCastHit)
        {
            PlayDualSquishAndStretch();
            indicatorProgressCapacity.Setup(0, duration *2 ,false, 0, 1, wood);
            RewardClaim();
            canClaim = false;
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(ChopWood());
        }

        private IEnumerator ChopWood()
        {
            yield return waitForDuration;
            _sawMill.Play();
            yield return waitForDuration;
            _sawMill.Stop();
            indicatorProgressCapacity.Change(1, 1);
            canClaim = true;
        }

        #endregion

        public bool canClaim;
        public bool CanClaim => canClaim;

        public void RewardClaim()
        {
            if(canClaim) playerFarm.inventory.AddOrIncrease(wood, 1);
        }
    }
}