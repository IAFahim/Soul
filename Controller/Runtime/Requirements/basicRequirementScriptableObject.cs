using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Requirements;
using UnityEngine;

namespace Soul.Controller.Runtime.Requirements
{
    [CreateAssetMenu(fileName = "Requirement", menuName = "Soul/Requirement/Create Requirement")]
    public class BasicRequirementScriptableObject : BasicRequiremntSO<Item, int>
    {
        
    }
}