using Soul.Model.Runtime.Modifiers;
using UnityEngine;

namespace Soul.Model.Runtime.Attacks
{
    public class AttackConfigForLevel : ScriptableObject
    {
        [SerializeField] private Modifier[] attackInfo;
    }
}