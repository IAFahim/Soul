namespace Soul.Model.Runtime.Unlocks
{
    public interface IUnlock
    {
        public bool IsUnlocking { get; }
        bool CanUnlock { get; }
        void Unlock();
    }
}