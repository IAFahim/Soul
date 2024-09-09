using Links.Runtime;
using LitMotion;
using Pancake.Common;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Indicators.Popups;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.RequiredAndRewards.Rewards;
using Soul.Model.Runtime.Tweens;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.SpritePopups
{
    public class PopupClickableIconCount : IndicatorPopUpClickable, ILoadComponent
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] protected TMPFormat countText;

        [SerializeField] private float jumpOutDuration = 0.5f;
        [SerializeField] private float jumpOutHeight = 20f;
        [SerializeField] private Ease jumpEase;
        private MotionHandle _jumpOutMotionHandle;
        
        [FormerlySerializedAs("onCollectReturnToPool")] [SerializeField]
        private bool onClickReturnToPool = true;

        private IRewardClaim _rewardClaimReference;
        private IReward<Pair<Item, int>> _rewardReference;
        
        [SerializeField] private ScriptableEventGetGameObject getCameraEvent;
        private GameObject _mainCamera;
        private bool _wasWaiting;

        private void OnEnable()
        {
            if (_mainCamera == null)
            {
                _mainCamera = getCameraEvent.Get();
                if (_mainCamera != null) OnCameraChange(_mainCamera);
            }

            if (_mainCamera != null) return;
            getCameraEvent.OnValueChange += OnCameraChange;
            _wasWaiting = true;
        }

        private void OnCameraChange(GameObject mainCamera)
        {
            transform.rotation = mainCamera.transform.rotation;
        }
        
        private void OnDisable()
        {
            if (_wasWaiting) getCameraEvent.OnValueChange -= OnCameraChange;
        }


        public Pair<Item, int> Reward => _rewardReference.Reward;

        public void Setup(IRewardClaim rewardClaim, IReward<Pair<Item, int>> reward,
            bool clickReturnToPool)
        {
            _rewardClaimReference = rewardClaim;
            _rewardReference = reward;
            onClickReturnToPool = clickReturnToPool;
            Reload();
            StartTween();
        }

        public void Reload()
        {
            Set(Reward.Key.icon, Reward.Value);
        }

        private void Set(Sprite sprite, int count)
        {
            spriteRenderer.sprite = sprite;
            countText.SetTextInt(count);
        }

        protected virtual void StartTween()
        {
            if (_jumpOutMotionHandle.IsActive()) _jumpOutMotionHandle.Cancel();
            _jumpOutMotionHandle = PureTween.TweenHeight(transform, jumpOutHeight, jumpOutDuration, jumpEase);
        }

        public override void OnSelected(RaycastHit selfRayCastHit)
        {
            if (_rewardClaimReference.CanClaim) _rewardClaimReference.RewardClaim();
            if (onClickReturnToPool) ReturnToPool();
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