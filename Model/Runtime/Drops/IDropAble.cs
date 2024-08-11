using _Root.Scripts.Model.Runtime.CustomList;

namespace _Root.Scripts.Model.Runtime.Drops
{
    public interface IDropAble<T>
    {
        public bool MultipleDropMode { get; }
        public bool CanDropNow { get; }
        public bool Drop(T[] thingToDrop);
        public ScriptableList<T> AllowedThingsToDrop { get; }
    }
}