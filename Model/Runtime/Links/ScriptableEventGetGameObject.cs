using UnityEngine;

namespace Soul.Model.Runtime.Links
{
    [CreateAssetMenu(fileName = "ScriptableEventGetGameObject", menuName = "Scriptable Objects/ScriptableEventGetGameObject")]
    public class ScriptableEventGetGameObject : ScriptableObject
    {
        public GameObject gameObject;
        public GameObject Raise() => gameObject;
    }
}
