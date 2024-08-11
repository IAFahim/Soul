using System;
using _Root.Scripts.Model.Runtime.AddressableHelper;
using _Root.Scripts.Model.Runtime.Interfaces;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Workers
{
    [CreateAssetMenu(fileName = ".worker", menuName = "Scriptable/Worker/Create Worker")]
    [Serializable]
    public class WorkerType : ScriptableObject, ITitle
    {
        [SerializeField] private string title;
        public string Title => title;
        public AsyncAssetReferenceGameObject asyncAssetReferenceGameObject;
    }
}