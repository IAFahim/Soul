using Pancake.UI;
using UnityEngine;
using SettingView = Soul.Presenter.Runtime.Views.SettingView;

namespace Soul.Presenter.Runtime.Popups
{
    [RequireComponent(typeof(SettingView))]
    public sealed class SettingPopup : Popup<SettingView>
    {
    }
}