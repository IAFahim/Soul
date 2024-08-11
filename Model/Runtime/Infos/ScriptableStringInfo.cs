using Soul.Model.Runtime.Interfaces;
using UnityEngine;

namespace Soul.Model.Runtime.Infos
{
    public class ScriptableStringInfo : ScriptableObject, ITitle, IDescription
    {
        [SerializeField] private string title;
        [TextArea(3, 5)] [SerializeField] private string description;
        
        public string Title => title;
        public string Description => description;
    }
}