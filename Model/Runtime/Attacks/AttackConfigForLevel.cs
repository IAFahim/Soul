using _Root.Scripts.Model.Runtime.Modifiers;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Attacks
{
    public class AttackConfigForLevel : ScriptableObject
    {
        [SerializeField] private Modifier[] attackInfo;
    }
}