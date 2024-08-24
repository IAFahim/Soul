using Soul.Model.Runtime.Levels;

namespace Soul.Model.Runtime.Upgrades
{
    public interface IUpgrade : ILevel
    {
        public bool CanUpgrade { get; }
        public bool IsUpgrading { get; }
        public void Upgrade();
    }
}