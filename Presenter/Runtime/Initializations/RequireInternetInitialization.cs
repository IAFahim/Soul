using System;
using Pancake;
using Pancake.Common;
using Pancake.UI;
using Soul.Presenter.Runtime.Popups;
using UnityEngine;

namespace Soul.Presenter.Runtime.Launchers
{
    [EditorIcon("icon_default")]
    [Serializable]
    public class RequireInternetInitialization : BaseInitialization
    {
        [SerializeField] private float timeCheckAgain = 5f;
        [SerializeField, PopupPickup] private string noInternetPopupKey;

        public override void Init()
        {
            if (!HeartSettings.RequireInternet) return;
            App.Delay(this, timeCheckAgain, OnUpdateCallback, isLooped: true);
        }

        private void OnUpdateCallback()
        {
            C.Network.CheckConnection(network =>
            {
                if (network != ENetworkStatus.Connected)
                {
                    var popupContainer = MainUIContainer.In.GetMain<PopupContainer>();
                    popupContainer.Popups.TryGetValue(noInternetPopupKey, out var popup);
                    if (popup == null) popupContainer.Push<NoInternetPopup>(noInternetPopupKey, true, popupId: noInternetPopupKey);
                }
            });
        }
    }
}