using Soul.Model.Runtime.CustomList;

namespace Soul.Model.Runtime.Drops
{
    public interface IDropAble<T>
    {
        public bool MultipleDropMode { get; }
        public ScriptableList<T> AllowedThingsToDrop { get; }
        public bool CanDropNow { get; }
        public bool DropHovering(T[] thingToDrop);
        public bool TryDrop(T[] thingToDrop);
    }
}
