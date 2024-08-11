using Soul.Model.Runtime.Levels;

namespace Soul.Controller.Runtime.Upgrades
{
    public interface IUpgrade
    {
        public Level Level { get; }
        public bool IsUpgrading { get; }
    }
}