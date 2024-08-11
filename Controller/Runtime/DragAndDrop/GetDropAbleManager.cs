using Pancake;
using Soul.Model.Runtime.CustomList;
using Soul.Model.Runtime.Drops;
using UnityEngine;

namespace Soul.Controller.Runtime.DragAndDrop
{
    public abstract class GetDropAbleManager<T> : GameComponent
    {
        [SerializeField] protected ScriptableList<T> currentAllowedThingsToDrop;
        [SerializeField] private CanvasGroup containerCanvasGroup;
        [SerializeField] private Transform containerTransform;

        public void OnSelect(Transform selectedTransform)
        {
            if (selectedTransform.TryGetComponent<IDropAble<T>>(out var dropAble))
            {
                if (dropAble.CanDropNow)
                {
                    containerCanvasGroup.alpha = 1;
                    currentAllowedThingsToDrop = dropAble.AllowedThingsToDrop;
                    CanDrop(currentAllowedThingsToDrop, containerTransform);
                }
                else
                {
                    containerCanvasGroup.alpha = 0;
                }
            }
        }

        public abstract void CanDrop(ScriptableList<T> allowedThingsToDrop, Transform container);
        public abstract void CantDrop();
    }
}