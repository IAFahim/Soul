using System;
using Alchemy.Inspector;
using LitMotion;
using Pancake;
using Pancake.Common;
using Pancake.Pools;
using Soul.Controller.Runtime.Tweens;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Selectors;
using TMPro;
using UnityEngine;

namespace Soul.Controller.Runtime.SpritePopups
{
    public abstract class PopupSpriteCount : GameComponent, ISelectCallBack, IPoolCallbackReceiver, ILoadComponent
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] protected TMPFormat countText;

        [SerializeField] private float jumpOutDuration = 0.5f;
        [SerializeField] private float jumpOutHeight = 20f;
        [SerializeField] private Ease jumpEase;
        private MotionHandle _jumpOutMotionHandle;

        protected bool FromPool;

        [Button]
        public void Setup(Sprite sprite, int count)
        {
            spriteRenderer.sprite = sprite;
            SetCount(count);

            if (_jumpOutMotionHandle.IsActive()) _jumpOutMotionHandle.Cancel();
            _jumpOutMotionHandle = Tween.JumpOut(transform, jumpOutHeight, jumpOutDuration, jumpEase);
        }

        public void SetCount(int count) => countText.SetTextInt(count);

        public abstract void OnSelected(RaycastHit selfRaycastHit);

        public void OnRequest()
        {
            FromPool = true;
        }

        public void ReturnToPool()
        {
            if (FromPool) GameObject.Return();
            else Destroy(gameObject);
        }

        public void OnReturn()
        {
        }

        void ILoadComponent.OnLoadComponents()
        {
            Reset();
        }

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            countText.TMP = GetComponentInChildren<TMP_Text>();
        }
    }
}