using Alchemy.Inspector;
using Pancake;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Selectors;
using UnityEngine;

namespace Soul.Controller.Runtime.SelectableComponents
{
    public abstract class BaseSelectableComponent : GameComponent, IGuid, ITitle, ISelectCallBack
    {
        [Title("Building")]
        [SerializeField, Guid] protected string guid;

        #region GUID

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        #endregion

        #region Title

        public abstract string Title { get; }

        #endregion

        #region ISelectCallBack

        public abstract void OnSelected(RaycastHit selfRayCastHit);

        #endregion
    }
}