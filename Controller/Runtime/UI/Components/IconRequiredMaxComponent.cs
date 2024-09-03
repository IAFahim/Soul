using Alchemy.Inspector;
using Coffee.UIEffects;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Soul.Controller.Runtime.UI.Components
{
    public class IconRequiredMaxComponent : MonoBehaviour
    {
        [SerializeField] private RectTransform container;
        [SerializeField] private Image image;
        [SerializeField] private TMPFormat countMaxTMPFormat;
        [SerializeField] private UIShiny uIShiny;

        [SerializeField] private Vector3 startingOffset = new(-350, 0, 0);
        private MotionHandle _motionHandle;
        public float notEnoughDuration = 0.4f;
        public float hasEnoughDuration = 0.3f;

        private void Awake()
        {
            countMaxTMPFormat.StoreFormat();
        }

        [Button]
        public void Setup(Sprite sprite, int required, int has)
        {
            image.sprite = sprite;
            countMaxTMPFormat.TMP.text = string.Format(countMaxTMPFormat, required, has);
            var remaining = has - required;
            var duration = remaining >= 0 ? hasEnoughDuration : notEnoughDuration;
            PlayAnimation(duration);
            PlayUIShiny(remaining, duration);
        }

        private void PlayAnimation(float duration)
        {
            if (_motionHandle.IsActive()) _motionHandle.Cancel();
            _motionHandle = LMotion.Create(startingOffset, Vector3.zero, duration).BindToAnchoredPosition3D(container);
        }
        private void PlayUIShiny(int remaining, float duration)
        {
            if (remaining >= 0)
            {
                uIShiny.effectPlayer.initialPlayDelay = duration;
                uIShiny.Play();
            }
            else uIShiny.effectFactor = 0;
        }
    }
}