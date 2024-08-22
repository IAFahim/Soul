using Pancake.Common;
using Soul.Controller.Runtime.FloatingUI;
using UnityEngine;

namespace Soul.Controller.Runtime.InfoPanels
{
    public abstract class InfoPanel : LookAtCamera, IInfoPanel, ILoadComponent
    {
        [SerializeField] protected int found;
        public abstract bool Setup(Transform mainCamera, Transform targetTransform);

        public abstract void AnimationReset();

        void ILoadComponent.OnLoadComponents()
        {
            OnLoadComponents();
        }

        protected abstract void OnLoadComponents();
    }
}