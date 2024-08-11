using Pancake;
using UnityEngine;

namespace Soul.Model.Runtime.Links
{
    public class GameObjectBridgeComponent : GameComponent
    {
        [SerializeField] private ScriptableEventGetGameObject getGameObjectEvent;
        [SerializeField] private GameObject target;

        protected void OnEnable()
        {
            getGameObjectEvent.gameObject = this.gameObject;
        }

        protected void OnDisable()
        {
            getGameObjectEvent.gameObject = null;
        }
    }
}