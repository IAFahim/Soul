using Pancake.UI;
using Soul.Presenter.Runtime.Views;
using UnityEngine;

namespace Soul.Presenter.Runtime.Popups
{
    [RequireComponent(typeof(NoInternetView))]
    public sealed class NoInternetPopup : Popup<NoInternetView>
    {
    }
}