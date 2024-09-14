using UnityEngine;

namespace Soul.Model.Runtime.Utils
{
    public static class GameObjectHelper
    {
        public static T GetComponentInChildrenWihtName<T>(this GameObject source,string name) where T : Component
        {
            var components = source.GetComponentsInChildren<T>();
            foreach (var component in components)
            {
                if (component.gameObject.name.ToLower().Contains(name))
                {
                    return component;
                }
            }

            return default;
        }
    }
}