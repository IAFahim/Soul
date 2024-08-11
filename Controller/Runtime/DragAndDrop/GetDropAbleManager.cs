using _Root.Scripts.Model.Runtime.CustomList;
using _Root.Scripts.Model.Runtime.Drops;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.DragAndDrop
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