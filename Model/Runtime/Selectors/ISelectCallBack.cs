using UnityEngine;

namespace Soul.Model.Runtime.Selectors
{
    public interface ISelectCallBack
    {
        public void OnSelected(RaycastHit selfRaycastHit);
    }
}