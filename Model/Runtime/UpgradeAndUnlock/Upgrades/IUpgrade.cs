using Soul.Model.Runtime.Levels;

namespace Soul.Model.Runtime.UpgradeAndUnlock.Upgrades
{
    public interface IUpgrade : ILevel, IUpgradeUnlockPreView
    {
        public bool CanUpgrade { get; }
        public bool IsUpgrading { get; }
        public void Upgrade();

        public void OnUnlockUpgradeStart();
        public void OnUnlockUpgradeComplete(int toLevel);
    }
}