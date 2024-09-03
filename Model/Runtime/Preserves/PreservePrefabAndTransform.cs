using System;
using Alchemy.Inspector;
using Pancake.Pools;
using Soul.Model.Runtime.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Soul.Model.Runtime.Preserves
{
    [Serializable]
    public class PreservePrefabAndTransform : PreserveTransformInfo
    {
        [Preview] public GameObject prefab;
        public bool usePool = true;

        public PreservePrefabAndTransform(GameObject gameObject, bool destroyObject = true)
        {
            Preserve(gameObject, destroyObject);
        }

        public PreservePrefabAndTransform(Transform transform, bool destroyObject = true)
        {
            Preserve(transform.gameObject, destroyObject);
        }

        public bool PoolOrInstantiate(Transform parent, out GameObject gameObject)
        {
            gameObject = usePool ? Request(parent) : Instantiate(parent);
            return usePool;
        }

        public bool ReturnOrDestroy(GameObject gameObject)
        {
            if (usePool) gameObject.Return();
            else gameObject.SafeDestroy();
            return usePool;
        }


        private GameObject Instantiate(Transform parent)
        {
            var gameObject = Object.Instantiate(prefab, parent);
            return SetTransformInfo(gameObject);
        }

        private GameObject Request(Transform parent)
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
            EditorUtility.SetDirty(gameObject.transform.parent);
#endif
            if (destroyObject) gameObject.SafeDestroy();
        }

        private bool FindAndLoadPrefab(GameObject gameObject)
        {
#if UNITY_EDITOR
            if (Application.isPlaying || gameObject == null) return false;
            var gameObjectNameWithoutClone = gameObject.name.Split("(Clone)")[0];
            var guids = AssetDatabase.FindAssets(gameObjectNameWithoutClone);

            string path = null;
            if (guids.Length > 1)
                foreach (var guid in guids)
                {
                    var paths = AssetDatabase.GUIDToAssetPath(guid);
                    if (!paths.EndsWith(".fbx"))
                    {
                        path = paths;
                        break;
                    }
                }
            else
                path = AssetDatabase.GUIDToAssetPath(guids[0]);


            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return true;
#else
            return false;
#endif
        }
    }
}