using _Root.Scripts.Model.Runtime.CustomList;

namespace _Root.Scripts.Model.Runtime.Drops
{
    public interface IDropRule<T>
    {
        public ScriptableList<T> AllowedThingsToDrop { get; }
    }
}