using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Selectors
{
    public interface IReSelectedCallBack
    {
        public void OnReSelected(RaycastHit selfReRaycastHit);
    }
}