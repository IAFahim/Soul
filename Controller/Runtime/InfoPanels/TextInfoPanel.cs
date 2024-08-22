using Soul.Model.Runtime.Interfaces;
using TMPro;
using UnityEngine;

namespace Soul.Controller.Runtime.InfoPanels
{
    public class TextInfoPanel : InfoPanel
    {
        [SerializeField] protected TMP_Text titleText;
        [SerializeField] protected Color titleColor = Color.white;
        
        public override bool Setup(Transform mainCamera, Transform targetTransform)
        {
            found = 0;
            if (targetTransform.TryGetComponent<ITitle>(out var titleComponent))
            {
                titleText.text = titleComponent.Title;
                found++;
            }

            cameraTransform = mainCamera;
            return found != 0;
        }

        public override void AnimationReset() => titleText.color = titleColor;

        protected override void OnLoadComponents()
        {
            titleText = GetComponentInChildren<TMP_Text>();
            titleColor = titleText.color;
        }
    }
}