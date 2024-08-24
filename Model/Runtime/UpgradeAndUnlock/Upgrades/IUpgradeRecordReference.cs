using Soul.Model.Runtime.Progressions;

namespace Soul.Model.Runtime.UpgradeAndUnlock.Upgrades
{
    public interface IUpgradeRecordReference<T> where T : class, IInProgression
    {
        public T UpgradeRecord { get; set; }
    }
}