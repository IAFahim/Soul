using UnityEngine;

namespace Soul.Model.Runtime.Selectors
{
    public interface IDeSelectedCallBack
    {
        public void OnDeSelected(RaycastHit otherRaycastHit);
    }
}