using System;
using Soul.Model.Runtime.AddressableHelper;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;

namespace Soul.Model.Runtime.Workers
{
    [CreateAssetMenu(fileName = ".worker", menuName = "Soul/Worker/Create Worker")]
    [Serializable]
    public class WorkerType : ScriptableObject, ITitle
    {
        [SerializeField] private string title;
        public string Title => title;
        public AsyncAssetReferenceGameObject asyncAssetReferenceGameObject;
    }
}