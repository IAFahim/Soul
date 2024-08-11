using Soul.Model.Runtime.CustomList;

namespace Soul.Model.Runtime.Drops
{
    public interface IDropAble<T>
    {
        public bool MultipleDropMode { get; }
        public bool CanDropNow { get; }
        public bool Drop(T[] thingToDrop);
        public ScriptableList<T> AllowedThingsToDrop { get; }
    }
}