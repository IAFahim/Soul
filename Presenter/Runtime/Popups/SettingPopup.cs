using _Root.Scripts.Presenter.Runtime.Views;
using Pancake.UI;
using UnityEngine;

namespace _Root.Scripts.Presenter.Runtime.Popups
{
    [RequireComponent(typeof(SettingView))]
    public sealed class SettingPopup : Popup<SettingView>
    {
    }
}