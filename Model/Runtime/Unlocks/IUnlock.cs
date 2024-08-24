namespace Soul.Model.Runtime.Unlocks
{
    public interface IUnlock
    {
        bool CanUnlock { get; }
        public bool IsUnlocking { get; }
        void Unlock();
    }
}