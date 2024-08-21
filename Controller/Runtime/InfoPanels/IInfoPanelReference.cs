using UnityEngine;

namespace Soul.Controller.Runtime.InfoPanels
{
    public interface IInfoPanelReference
    {
        public IInfoPanel InfoPanelPrefab { get; }
        public Vector3 InfoPanelWorldPosition { get; }
    }
}