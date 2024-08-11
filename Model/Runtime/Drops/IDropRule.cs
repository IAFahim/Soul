using Soul.Model.Runtime.CustomList;

namespace Soul.Model.Runtime.Drops
{
    public interface IDropRule<T>
    {
        public ScriptableList<T> AllowedThingsToDrop { get; }
    }
}