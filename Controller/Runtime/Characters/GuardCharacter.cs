using Pancake.Pools;
using Soul.Controller.Runtime.Defence;
using Soul.Model.Runtime.Stats;
using UnityEngine;

namespace Soul.Controller.Runtime.Characters
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