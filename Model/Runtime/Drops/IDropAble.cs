namespace Soul.Model.Runtime.Drops
{
    public interface IDropAble<in TDrop>
    {
        public bool MultipleDropMode { get; }
        public bool CanDropNow { get; }
        public bool DropHovering(TDrop thingToDrop);
        public bool TryDrop(TDrop dropPackage);
    }
}