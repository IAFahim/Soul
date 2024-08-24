using Pancake;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Selectors;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings
{
    public abstract class Building : GameComponent, IGuid, ITitle, ISelectCallBack
    {
        #region GUID

        [SerializeField, Guid] protected string guid;

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