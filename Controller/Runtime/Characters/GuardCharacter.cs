using _Root.Scripts.Controller.Runtime.Characters;
using _Root.Scripts.Controller.Runtime.Components;
using _Root.Scripts.Controller.Runtime.Defence;
using _Root.Scripts.Model.Runtime.Stats;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime
{
    public class GuardCharacter : CharacterComponent, IHealthReference
    {
        public Health health;
        [SerializeField] private int index;
        [SerializeField] private DefenceLineSegment assignedSegment;

        public Health Health => health;
        public void Request(int lineIndex, DefenceLineSegment lineSegment,DefenceLine defenceLine)
        {
            GuardCharacter guardCharacter = GameObject.Request<GuardCharacter>();
            guardCharacter.index = lineIndex;
            guardCharacter.assignedSegment = lineSegment;
        }
    }
}