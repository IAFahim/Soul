using UnityEditor;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Extensions
{
    public static class UnityObjectExtension
    {
        public static void SafeDestroy(this Object obj)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorApplication.delayCall += () => { Object.DestroyImmediate(obj); };
                EditorUtility.SetDirty(obj);
                return;
            }
#endif
            Object.Destroy(obj);
        }
    }
}