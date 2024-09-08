using Alchemy.Inspector;
using Coffee.UIEffects;
using LitMotion;
using LitMotion.Extensions;
using Soul.Model.Runtime.PoolAbles;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Soul.Controller.Runtime.UI.Components
{
    public class ContainerIconRequiredMax : PoolAbleComponent
    {
        [SerializeField] private RectTransform container;

        [SerializeField] private Image icon;

        [SerializeField] private Image background;
        [SerializeField] private TMPFormat countMaxTMPFormat;
        [SerializeField] private UIShiny uIShiny;
        [FormerlySerializedAs("clear")] [SerializeField] private Color clearColor = Color.clear;

        [SerializeField] private Vector3 startingOffset = new(-350, 0, 0);
        [SerializeField] private Ease ease = Ease.Linear;
        [SerializeField] private float notEnoughDuration = 0.4f;
        [SerializeField] private float blinkMultiplier = 1f;
        [SerializeField] private float hasEnoughDuration = 0.3f;

        private Color _startingColor;
        private MotionHandle _slideMotionHandle;
        private MotionHandle _blinkMotionHandle;

        private void Awake()
        {
            _startingColor = background.color;
            countMaxTMPFormat.StoreFormat();
        }

        [Button]
        public void Setup(Sprite sprite, int required, int has)
        {
            bool hasEnough = has - required >= 0;
            var duration = SetValues(sprite, required, has);
            PlayAnimation(duration);
        }

        public float SetValues(Sprite sprite, int required, int has)
        {
            bool hasEnough = has - required >= 0;
            var duration = hasEnough ? hasEnoughDuration : notEnoughDuration;
            icon.sprite = sprite;
            countMaxTMPFormat.TMP.text = string.Format(countMaxTMPFormat, required, has);
            PlayUIShinyBlink(hasEnough, duration);
            return duration;
        }

        private void PlayAnimation(float duration)
        {
            if (_slideMotionHandle.IsActive()) _slideMotionHandle.Cancel();
            _slideMotionHandle = LMotion.Create(startingOffset, Vector3.zero, duration)
                .BindToAnchoredPosition3D(container);
        }

        private void PlayUIShinyBlink(bool hasEnough, float duration)
        {
            if (_blinkMotionHandle.IsActive()) _blinkMotionHandle.Cancel();
            
            if (hasEnough)
            {
                uIShiny.effectPlayer.initialPlayDelay = duration;
                uIShiny.Play();
            }
            else
            {
                uIShiny.effectFactor = 0;
                _blinkMotionHandle = LMotion.Create(_startingColor, clearColor, duration * blinkMultiplier)
                    .WithLoops(-1, LoopType.Yoyo)
                    .WithDelay(duration)
                    .WithEase(ease)
                    .BindToColor(background);
            }
        }

        public override void OnReturn()
        {
            if (_slideMotionHandle.IsActive()) _slideMotionHandle.Cancel();
            if (_blinkMotionHandle.IsActive()) _blinkMotionHandle.Cancel();
            container.anchoredPosition3D = startingOffset;
        }
    }
}