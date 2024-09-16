using System;
using LitMotion;
using LitMotion.Extensions;
using Soul.Controller.Runtime.UI;
using Soul.Model.Runtime.Reactives;
using Soul.Model.Runtime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Presenter.Runtime.UI.StatsViews
{
    [Serializable]
    public class CurrencyViewUI : PreviewStats
    {
        [FormerlySerializedAs("coinText")] [SerializeField] protected TMPFormat currentCurrencyText;
        
        
        private MotionHandle _coinIncreaseMotionHandle;
        public void Setup(ReactiveSaveAble<int> coinReference, Reactive<int> coinPreview)
        {
            SetupPreview(coinPreview);
            SetupToggle();
            coinReference.OnChange += UpdateCoin;
            UpdateCoin(0, coinReference.Value);
        }
       

        private void UpdateCoin(int old, int newValue)
        {
            if (_coinIncreaseMotionHandle.IsActive()) _coinIncreaseMotionHandle.Cancel();
            _coinIncreaseMotionHandle = LMotion.Create(old, newValue, toggleDuration).WithEase(toggleEase)
                .BindToText(currentCurrencyText);
        }

        public override GameObject LoadComponents(GameObject gameObject, string title)
        {
            var container= base.LoadComponents(gameObject, title);
            currentCurrencyText.TMP = container.GetComponentInChildrenWihtName<TMP_Text>("current");
            return container;
        }
    }
}