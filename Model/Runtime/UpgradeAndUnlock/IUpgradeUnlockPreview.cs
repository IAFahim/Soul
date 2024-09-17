using UnityEngine;

namespace Soul.Model.Runtime.UpgradeAndUnlock
{
    public interface IUpgradeUnlockPreview
    {
        public void ShowUpgradeUnlockPreview(RectTransform parent);
        public void HideUpgradeUnlockPreview();
    }
}