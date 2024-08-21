using LitMotion;
using LitMotion.Extensions;
using Pancake.Common;
using Soul.Controller.Runtime.FloatingUI;
using Soul.Model.Runtime.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.InfoPanels
{
    public class InfoPanel : LookAtCamera, IInfoPanel,ILoadComponent
    {
        [FormerlySerializedAs("title")] [SerializeField]
        private TMP_Text titleText;

        [SerializeField] Color titleColor = Color.white;

        [SerializeField] SpriteRenderer iconSpriteRenderer;
        [SerializeField] Color iconColor = Color.white;
        [SerializeField] SpriteRenderer backgroundSpriteRenderer;
        [SerializeField] Color backgroundColor = Color.white;

        [SerializeField] int found;

        public bool Setup(Transform mainCamera, Transform targetTransform)
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
        
        public void FadingReset()
        {
            titleText.color = titleColor;
            backgroundSpriteRenderer.color = backgroundColor;
            iconSpriteRenderer.color = iconColor;
        }

        void ILoadComponent.OnLoadComponents()
        {
            OnLoadComponents();
        }

        protected virtual void OnLoadComponents()
        {
            titleText = GetComponentInChildren<TMP_Text>();
            iconSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            backgroundSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[^1];

            titleText.color = titleColor;
            iconSpriteRenderer.color = iconColor;
            backgroundSpriteRenderer.color = backgroundColor;
        }
    }
}