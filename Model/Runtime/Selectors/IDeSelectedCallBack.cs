using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Selectors
{
    public interface IDeSelectedCallBack
    {
        public void OnDeSelected(RaycastHit otherRaycastHit);
    }
}