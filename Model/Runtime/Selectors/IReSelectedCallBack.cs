using UnityEngine;

namespace Soul.Model.Runtime.Selectors
{
    public interface IReSelectedCallBack
    {
        public void OnReSelected(RaycastHit selfReRaycastHit);
    }
}