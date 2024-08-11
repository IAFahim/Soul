using System;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Containers
{
    [Serializable]
    public class PositionRotation
    {
        public Vector3 position;
        public Quaternion rotation;

        public static implicit operator Vector3(PositionRotation positionRotation)
        {
            return positionRotation.position;
        }

        public static implicit operator Quaternion(PositionRotation positionRotation)
        {
            return positionRotation.rotation;
        }
    }
}