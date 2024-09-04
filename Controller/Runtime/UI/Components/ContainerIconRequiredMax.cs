﻿using Alchemy.Inspector;
using Coffee.UIEffects;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Soul.Controller.Runtime.UI.Components
{
    public class ContainerIconRequiredMax : MonoBehaviour
    {
        [SerializeField] private RectTransform container;

        [FormerlySerializedAs("image")] [SerializeField]
        private Image icon;

        [SerializeField] private Image background;
        [SerializeField] private TMPFormat countMaxTMPFormat;
        [SerializeField] private UIShiny uIShiny;

        [SerializeField] private Vector3 startingOffset = new(-350, 0, 0);
        [SerializeField] private Ease ease = Ease.Linear;
        [SerializeField] private float notEnoughDuration = 0.4f;
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
            PlayAnimation(hasEnough, duration);
        }

        public float SetValues(Sprite sprite, int required, int has)
        {
            bool hasEnough = has - required >= 0;
            var duration = hasEnough ? hasEnoughDuration : notEnoughDuration;
            icon.sprite = sprite;
            countMaxTMPFormat.TMP.text = string.Format(countMaxTMPFormat, required, has);
            PlayUIShiny(hasEnough, duration);
            return duration;
        }

        private void PlayAnimation(bool hasEnough, float duration)
        {
            if (_slideMotionHandle.IsActive()) _slideMotionHandle.Cancel();
            _slideMotionHandle = LMotion.Create(startingOffset, Vector3.zero, duration)
                .BindToAnchoredPosition3D(container);
            
            if (_blinkMotionHandle.IsActive()) _blinkMotionHandle.Cancel();
            if (!hasEnough)
            {
                _blinkMotionHandle = LMotion.Create(_startingColor, Color.clear, duration / 2)
                    .WithLoops(2, LoopType.Yoyo)
                    .WithDelay(duration)
                    .WithEase(ease)
                    .BindToColor(background);
            }
        }

        private void PlayUIShiny(bool hasEnough, float duration)
        {
            if (hasEnough)
            {
                uIShiny.effectPlayer.initialPlayDelay = duration;
                uIShiny.Play();
            }
            else uIShiny.effectFactor = 0;
        }
    }
}