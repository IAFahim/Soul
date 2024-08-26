namespace Soul.Model.Runtime.DragAndDrops
{
    public interface IDropAble<in TDrop>
    {
        public bool CanDropNow { get; }
        public bool OnDragStart(TDrop drop);
        public bool OnDrag(TDrop drop);
        public bool OnDrop(TDrop dropPackage);
        
        /// <summary>
        /// When the drag is canceled or goes out of bounds
        /// </summary>
        public void OnDragCancel();
    }
}