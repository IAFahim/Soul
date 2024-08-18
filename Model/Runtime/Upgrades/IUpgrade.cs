using Soul.Model.Runtime.Levels;

namespace Soul.Model.Runtime.Upgrades
{
    public interface IUpgrade : ILevel
    {
        public bool IsUpgrading { get; }
        public bool CanUpgrade { get; }
        public void Upgrade();
    }
}