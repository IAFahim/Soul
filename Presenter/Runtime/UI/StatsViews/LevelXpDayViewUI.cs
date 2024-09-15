using System;
using Alchemy.Inspector;
using LitMotion;
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
    public class LevelXpDayViewUI : StatsView
    {
        [Title("LevelXpDayViewUI")] [SerializeField]
        private Image xpBar;

        [SerializeField] private TMPFormat levelText;
        [SerializeField] private TMPFormat xpNextLevelText;
        [SerializeField] private TMPFormat dayText;

        [SerializeField] private CanvasGroup previewCanvasGroup;
        [SerializeField] private TMPFormat previewXpText;

        private MotionHandle _toggleMotionHandle;

        private LevelXp _levelXp;
        private Reactive<float> _xpPreview;
        private DelayHandle _xpPreviewDelayHandle;

        public void Setup(LevelXp levelXpReference, Reactive<float> xpPreviewReference)
        {
            StoreFormat();
            _levelXp = levelXpReference;
            _xpPreview = xpPreviewReference;
            _levelXp.OnXpChange += LevelOnXpChange;
            _levelXp.Load();
            LevelOnXpChange(_levelXp.Xp);
            dayText.SetTextFloat(UserData.DaySinceInstall + 1);
            _xpPreview.OnChange += UpdateXpPreview;
            ToggleSetup();
        }

        private void LevelOnXpChange(int currentXp)
        {
            levelText.SetTextInt(_levelXp.Current);
            var xpNextLevel = _levelXp.XpToNextLevel;
            xpNextLevelText.SetTextFloat(xpNextLevel - currentXp);
            xpBar.fillAmount = _levelXp.XpProgress;
            UpdateXpPreview(0, _xpPreview.Value);
            if (_xpPreviewDelayHandle is { IsCompleted: false }) _xpPreviewDelayHandle.Cancel();
            _xpPreviewDelayHandle = App.Delay(.4f, OnXpPreViewDurationComplete);
        }

        private void OnXpPreViewDurationComplete()
        {
            UpdateXpPreview(0, 0);
        }

        private void UpdateXpPreview(float oldValue, float newValue)
        {
            if (Mathf.Approximately(newValue, 0))
            {
                previewCanvasGroup.alpha = 0;
                return;
            }

            previewCanvasGroup.alpha = 1;
            previewXpText.SetTextFloat(newValue);
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
            _xpPreview.OnChange -= UpdateXpPreview;
        }

        public override GameObject LoadComponents(GameObject gameObject, string title)
        {
            var container = base.LoadComponents(gameObject, title);
            xpBar = container.GetComponentInChildrenWihtName<Image>("fill");
            levelText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("level");
            xpNextLevelText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("next");
            dayText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("day");
            previewCanvasGroup = container.GetComponentInChildrenWihtName<CanvasGroup>("xp");
            previewXpText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("xp");
            return container;
        }
    }
}