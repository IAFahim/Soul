using Alchemy.Inspector;
using LitMotion;
using Soul.Controller.Runtime.InfoPanels;
using Soul.Controller.Runtime.SelectableComponents;
using Soul.Model.Runtime.Buildings;
using Soul.Model.Runtime.Tweens;
using Soul.Model.Runtime.Tweens.Scriptable;
using UnityEngine;

namespace Soul.Presenter.Runtime.Vehicles
{
    public class Vehicle : BaseSelectableComponent, IInfoPanelReference
    {
        [Title("Vehicle")] [SerializeField] protected LevelInfrastructureInfo infrastructureInfo;
        public InfoPanel infoPanel;

        public IInfoPanel InfoPanelPrefab => infoPanel;

        public Vector3 InfoPanelWorldPosition =>
            transform.TransformPoint(infrastructureInfo.GetInfoPanelPositionOffset(0));

        public override string Title => infrastructureInfo.Title;

        #region Selected Animation

        [SerializeField] protected TweenSettingCurveScriptableObject<Vector3> selectTweenSetting;
        protected MotionHandle SelectTweenMotionHandle;

        protected void PlayDualSquishAndStretch()
        {
            if (SelectTweenMotionHandle.IsActive()) SelectTweenMotionHandle.Cancel();
            SelectTweenMotionHandle = transform.TweenScale(selectTweenSetting);
        }

        public override void OnSelected(RaycastHit selfRayCastHit)
        {
            PlayDualSquishAndStretch();
        }

        #endregion
    }
}