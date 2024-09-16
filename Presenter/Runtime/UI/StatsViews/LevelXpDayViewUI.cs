using System;
using Alchemy.Inspector;
using LitMotion;
using LitMotion.Extensions;
using Pancake.Common;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Reactives;
using Soul.Model.Runtime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.UI.StatsViews
{
    [Serializable]
    public class LevelXpDayViewUI : PreviewStats
    {
        [Title("LevelXpDayViewUI")] [SerializeField]
        private Image xpBar;

        [SerializeField] private TMPFormat levelText;
        [SerializeField] private TMPFormat xpNextLevelText;
        [SerializeField] private TMPFormat dayText;

        [SerializeField] private TMPFormat previewXpText;

        private MotionHandle _toggleMotionHandle;
        private MotionHandle _xpIncreaseMotionHandle;

        private LevelXp _levelXp;
        private DelayHandle _xpPreviewDelayHandle;

        public void Setup(LevelXp levelXpReference, Reactive<int> xpPreviewReference)
        {
            StoreFormat();
            SetupToggle();
            SetupPreview(xpPreviewReference);
            _levelXp = levelXpReference;
            _levelXp.OnXpChange += LevelOnXpChange;
            _levelXp.Load();
            LevelOnXpChange(0, _levelXp.Xp);
            dayText.SetTextFloat(UserData.DaySinceInstall + 1);
            
        }

        private void LevelOnXpChange(int oldXp, int currentXp)
        {
            levelText.SetTextInt(_levelXp.Current);
            xpBar.fillAmount = _levelXp.XpProgress;
            UpdatePreview(0, PreviewReference.Value);
            if (_xpIncreaseMotionHandle.IsActive()) _xpIncreaseMotionHandle.Cancel();
            _xpIncreaseMotionHandle = LMotion.Create(oldXp, currentXp, toggleDuration).WithEase(toggleEase)
                .BindToText(xpNextLevelText);
            if (_xpPreviewDelayHandle is { IsCompleted: false }) _xpPreviewDelayHandle.Cancel();
            _xpPreviewDelayHandle = App.Delay(toggleDuration, OnXpPreViewDurationComplete);
        }


        private void OnXpPreViewDurationComplete()
        {
            UpdatePreview(0, 0);
        }


        private void StoreFormat()
        {
            dayText.StoreFormat();
            levelText.StoreFormat();
            xpNextLevelText.StoreFormat();
            previewXpText.StoreFormat();
        }

        public override void Dispose()
        {
            base.Dispose();
            _levelXp.OnXpChange -= LevelOnXpChange;
        }

        public override GameObject LoadComponents(GameObject gameObject, string title)
        {
            var container = base.LoadComponents(gameObject, title);
            xpBar = container.GetComponentInChildrenWihtName<Image>("fill");
            levelText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("level");
            xpNextLevelText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("next");
            dayText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("day");
            return container;
        }
    }
}