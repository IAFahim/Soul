using UnityEngine;

namespace Soul.Model.Runtime.UpgradeAndUnlock
{
    public interface IUpgradeUnlockPreView
    {
        public void ShowUpgradeUnlockPreView(RectTransform parent);
        public void HideUpgradeUnlockPreView();
    }
}