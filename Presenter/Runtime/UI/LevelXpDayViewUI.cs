using System;
using LitMotion;
using LitMotion.Extensions;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Reactives;
using Soul.Model.Runtime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.UI
{
    [Serializable]
    public class LevelXpDayViewUI
    {
        [SerializeField] private bool isExpanded = true;
        [SerializeField] private LevelXp levelXp;

        [SerializeField] private RectTransform toggleExpandedRect;
        [SerializeField] private Button toggleExpandedButton;
        [SerializeField] private Image xpBar;
        [SerializeField] private TMPFormat levelText;
        [SerializeField] private TMPFormat xpNextLevelText;
        [SerializeField] private TMPFormat dayText;

        [SerializeField] private CanvasGroup previewCanvasGroup;
        [SerializeField] private TMPFormat previewXpText;

        [SerializeField] private Vector3 toggleStartPosition;
        [SerializeField] private Vector3 toggleEndPosition;
        [SerializeField] private float toggleDuration;
        [SerializeField] private Ease toggleEase;
        private MotionHandle _toggleMotionHandle;
        
        private Reactive<float> _xpPreview;

        public void Setup(LevelXp levelXpReference, Reactive<float> xpPreviewReference)
        {
            toggleStartPosition = toggleExpandedRect.anchoredPosition3D;
            _xpPreview = xpPreviewReference;
            levelXp = levelXpReference;
            StoreFormat();
            levelXp.OnXpChange += LevelXpOnXpChange;
            levelXp.AddXp(0);
            dayText.SetTextFloat(UserData.DaySinceInstall + 1);
            _xpPreview.OnChange += UpdateXpPreview;
            ToggleSetup();
        }

        private void LevelXpOnXpChange(int currentXp)
        {
            levelText.SetTextInt(levelXp.Current);
            var xpNextLevel = levelXp.XpToNextLevel;
            xpNextLevelText.SetTextFloat(xpNextLevel-currentXp);
            xpBar.fillAmount = levelXp.XpProgress;
        }

        private void ToggleSetup()
        {
            toggleExpandedButton.onClick.AddListener(ToggleExpand);
            var expandedInt = PlayerPrefs.GetInt("isExpanded", 1);
            isExpanded = expandedInt == 1;
            toggleExpandedRect.anchoredPosition3D = isExpanded ? toggleStartPosition : toggleEndPosition;
        }

        private void ToggleExpand()
        {
            isExpanded = !isExpanded;
            PlayerPrefs.SetInt("isExpanded", isExpanded ? 1 : 0);
            var startPosition = isExpanded ? toggleEndPosition : toggleStartPosition;
            var endPosition = isExpanded ? toggleStartPosition : toggleEndPosition;
            if (_toggleMotionHandle.IsActive()) _toggleMotionHandle.Cancel();
            _toggleMotionHandle = LMotion.Create(startPosition, endPosition, toggleDuration)
                .WithEase(toggleEase)
                .BindToAnchoredPosition3D(toggleExpandedRect);
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

        public void Dispose()
        {
            if(_toggleMotionHandle.IsActive()) _toggleMotionHandle.Cancel();
            levelXp.OnXpChange -= LevelXpOnXpChange;
            _xpPreview.OnChange -= UpdateXpPreview;
        }

        public void LoadComponents(GameObject gameObject)
        {
            toggleExpandedButton = gameObject.GetComponentInChildrenWihtName<Button>("level");
            toggleExpandedRect = gameObject.GetComponentInChildrenWihtName<RectTransform>("level");
            toggleStartPosition = toggleExpandedRect.anchoredPosition3D;
            toggleEndPosition = toggleStartPosition + Vector3.up * 90;
            xpBar = gameObject.GetComponentInChildrenWihtName<Image>("fill");
            levelText.TMP = gameObject.GetComponentInChildrenWihtName<TMP_Text>("level");
            xpNextLevelText.TMP = gameObject.GetComponentInChildrenWihtName<TMP_Text>("next");
            dayText.TMP = gameObject.GetComponentInChildrenWihtName<TMP_Text>("day");
            previewCanvasGroup = gameObject.GetComponentInChildrenWihtName<CanvasGroup>("xp");
            previewXpText.TMP = gameObject.GetComponentInChildrenWihtName<TMP_Text>("xp");
        }
    }
}