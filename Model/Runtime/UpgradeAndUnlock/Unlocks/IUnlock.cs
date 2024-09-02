namespace Soul.Model.Runtime.UpgradeAndUnlock.Unlocks
{
    public interface IUnlock
    {
        bool CanUnlock { get; }
        public bool IsUnlocking { get; }
        void Unlock();
    }
}