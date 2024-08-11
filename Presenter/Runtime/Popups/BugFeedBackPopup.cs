using _Root.Scripts.Presenter.Runtime.Views;
using Pancake.UI;
using UnityEngine;

namespace _Root.Scripts.Presenter.Runtime.Popups
{
    [RequireComponent(typeof(BugFeedBackView))]
    public class BugFeedBackPopup : Popup<BugFeedBackView>
    {
    }
}