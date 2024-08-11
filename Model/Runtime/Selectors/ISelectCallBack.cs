using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Selectors
{
    public interface ISelectCallBack
    {
        public void OnSelected(RaycastHit selfRaycastHit);
    }
}