using Alchemy.Inspector;
using LitMotion;
using LitMotion.Extensions;
using Pancake;
using Pancake.Common;
using Pancake.Pools;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Limits;
using Soul.Model.Runtime.PoolAbles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityProgressBar;

namespace Soul.Presenter.Runtime.UI
{
    public class RewardNotifier : PoolAbleComponent, ILoadComponent
    {
        [SerializeField] TMPFormat addedText;
        [SerializeField] TMPFormat totalText;
        [SerializeField] Image icon;
        [SerializeField] ProgressBar progressBar;
        [SerializeField, DisableInEditMode] Item itemReference;
        [SerializeField] float duration = 3f;
        [SerializeField] Ease ease = Ease.Linear;

        private IRemoveCallBack<Item> _removeCallBack;
        private MotionHandle _textIncreaseMotionHandle;


        private void Awake()
        {
            addedText.StoreFormat();
            totalText.StoreFormat();
        }

        public void Setup(Item item, int newAmount, int added, Limit limitInt,
            IRemoveCallBack<Item> removeCallBack)
        {
            if (_textIncreaseMotionHandle.IsActive()) _textIncreaseMotionHandle.Cancel();
            _textIncreaseMotionHandle = LMotion.Create(newAmount - added, newAmount, duration / 3f).WithEase(ease)
                .BindToText(totalText);
            addedText.TMP.SetText("+" + addedText, added);

            icon.sprite = item.Icon;
            progressBar.Value = limitInt.Progress;
            itemReference = item;
            _removeCallBack = removeCallBack;
            App.Delay(3f, OnComplete);
        }

        private void OnComplete()
        {
            _removeCallBack.RemoveSelf(itemReference);
        }

        void ILoadComponent.OnLoadComponents()
        {
            addedText.TMP = GetComponentInChildren<TMP_Text>();
            totalText.TMP = GetComponentsInChildren<TMP_Text>()[^1];
            icon = GetComponents<Image>()[^1];
            addedText.StoreFormat();
            totalText.StoreFormat();
        }
    }
}