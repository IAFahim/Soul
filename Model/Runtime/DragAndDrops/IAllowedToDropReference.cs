using System.Collections.Generic;

namespace Soul.Model.Runtime.DragAndDrops
{
    public interface IAllowedToDropReference<T>
    {
        
        public IList<T> ListOfAllowedToDrop { get; }
    }
}