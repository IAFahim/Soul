using System;
using Alchemy.Inspector;
using Pancake.Pools;
using Soul.Model.Runtime.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Soul.Model.Runtime.Preserves
{
    [Serializable]
    public class PreservePrefabAndTransform : PreserveTransformInfo
    {
        [Preview] public GameObject prefab;
        public PreservePrefabAndTransform(GameObject gameObject, bool destroyObject = true)
        {
            Preserve(gameObject, destroyObject);
        }
        
        public PreservePrefabAndTransform(Transform transform, bool destroyObject = true)
        {
            Preserve(transform.gameObject, destroyObject);
        }


        public GameObject Instantiate(Transform parent)
        {
            var gameObject = Object.Instantiate(prefab, parent);
            return SetTransformInfo(gameObject);
        }

        public GameObject Request(Transform parent)
        {
            var gameObject = prefab.Request(parent);
            return SetTransformInfo(gameObject);
        }
        
        [Button]
        protected void PreservePrefabAsEntry()
        {
            Preserve(prefab, true);
        }
        
        private void Preserve(GameObject gameObject, bool destroyObject)
        {
            var foundPrefab = FindAndLoadPrefab(gameObject);
            if (!foundPrefab) return;
            LoadValueFrom(gameObject.transform);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject.transform.parent);
#endif
            if (destroyObject) gameObject.SafeDestroy();
        }

        private bool FindAndLoadPrefab(GameObject gameObject)
        {
#if UNITY_EDITOR
            if (Application.isPlaying || gameObject == null) return false;
            var gameObjectNameWithoutClone = gameObject.name.Split("(Clone)")[0];
            var guids = UnityEditor.AssetDatabase.FindAssets(gameObjectNameWithoutClone);

            string path = null;
            if (guids.Length > 1)
                foreach (var guid in guids)
                {
                    var paths = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    if (!paths.EndsWith(".fbx"))
                    {
                        path = paths;
                        break;
                    }
                }
            else
                path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);

            
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return true;
#else
            return false;
#endif
        }
    }
}