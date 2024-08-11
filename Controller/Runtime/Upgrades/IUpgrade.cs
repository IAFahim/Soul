using _Root.Scripts.Model.Runtime.Levels;

namespace _Root.Scripts.Controller.Runtime.Upgrades
{
    public interface IUpgrade
    {
        public Level Level { get; }
        public bool IsUpgrading { get; }
    }
}