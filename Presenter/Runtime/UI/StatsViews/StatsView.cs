using System;
using Alchemy.Inspector;
using LitMotion;
using LitMotion.Extensions;
using Soul.Model.Runtime.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Soul.Presenter.Runtime.UI.StatsViews
{
    [Serializable]
    public class StatsView
    {
        [FormerlySerializedAs("toggleExpandedRect")]
        [Title("StatsView")]
        [SerializeField] protected RectTransform toggleExpandRect;
        [FormerlySerializedAs("toggleExpandedButton")] [SerializeField] protected Button toggleExpandButton;
        
        [SerializeField] protected bool isExpanded = true;
        [SerializeField] protected Vector3 toggleStartPosition;
        [SerializeField] protected Vector3 toggleEndPosition;
        [SerializeField] protected float toggleDuration;
        [SerializeField] protected Ease toggleEase;
        protected MotionHandle ToggleMotionHandle;
        
        protected void ToggleSetup()
        {
            toggleExpandButton.onClick.AddListener(ToggleExpand);
            var expandedInt = PlayerPrefs.GetInt("isExpanded", 1);
            isExpanded = expandedInt == 1;
            toggleExpandRect.anchoredPosition3D = isExpanded ? toggleStartPosition : toggleEndPosition;
        }
        
        protected void ToggleExpand()
        {
            isExpanded = !isExpanded;
            PlayerPrefs.SetInt("isExpanded", isExpanded ? 1 : 0);
            var startPosition = isExpanded ? toggleEndPosition : toggleStartPosition;
            var endPosition = isExpanded ? toggleStartPosition : toggleEndPosition;
            if (ToggleMotionHandle.IsActive()) ToggleMotionHandle.Cancel();
            ToggleMotionHandle = LMotion.Create(startPosition, endPosition, toggleDuration)
                .WithEase(toggleEase)
                .BindToAnchoredPosition3D(toggleExpandRect);
        }
        
        public virtual GameObject LoadComponents(GameObject gameObject, string title)
        {
            toggleExpandRect = gameObject.GetComponentInChildrenWihtName<RectTransform>(title);
            var container = toggleExpandRect.gameObject;
            toggleStartPosition = toggleExpandRect.anchoredPosition3D;
            toggleEndPosition = toggleStartPosition + Vector3.up * 90;
            
            toggleExpandButton = container.GetComponent<Button>();
            return container;
        }
        
        public virtual void Dispose()
        {
            if(ToggleMotionHandle.IsActive()) ToggleMotionHandle.Cancel();
        }
    }
}