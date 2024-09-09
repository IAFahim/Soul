using Pancake.UI;
using Soul.Presenter.Runtime.Views;
using UnityEngine;

namespace Soul.Presenter.Runtime.Popups
{
    [RequireComponent(typeof(NotificationView))]
    public sealed class NotificationPopup : Popup<NotificationView>
    {
    }
}
