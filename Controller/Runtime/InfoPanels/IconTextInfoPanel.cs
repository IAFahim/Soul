using Soul.Model.Runtime.Interfaces;
using TMPro;
using UnityEngine;

namespace Soul.Controller.Runtime.InfoPanels
{
    public class IconTextInfoPanel : InfoPanel
    {
        [SerializeField] protected TMP_Text titleText;
        [SerializeField] protected Color titleColor = Color.white;
        
        [SerializeField] SpriteRenderer iconSpriteRenderer;
        [SerializeField] Color iconColor = Color.white;
        
        public override bool Setup(Transform mainCamera, Transform targetTransform)
        {
            found = 0;
            if (targetTransform.TryGetComponent<ITitle>(out var titleComponent))
            {
                titleText.text = titleComponent.Title;
                found++;
            }

            if (targetTransform.TryGetComponent<IIcon>(out var iconComponent))
            {
                iconSpriteRenderer.sprite = iconComponent.Icon;
                found++;
            }

            cameraTransform = mainCamera;
            return found != 0;
        }

        public override void AnimationReset()
        {
            titleText.color = titleColor;
            iconSpriteRenderer.color = iconColor;
        }

        protected override void OnLoadComponents()
        {
            titleText = GetComponentInChildren<TMP_Text>();
            titleColor = titleText.color;
            iconSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            iconColor = iconSpriteRenderer.color;
        }
    }
}