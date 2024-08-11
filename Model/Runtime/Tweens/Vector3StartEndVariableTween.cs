using System;
using LitMotion;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Tweens
{
    [Serializable]
    public abstract class Vector3StartEndVariableTween : VariableTween
    {
        public Optional<Vector3> start;
        public Vector3 end;

        public abstract override MotionHandle Play(Transform transform);

        public override float CalculateDuration()
        {
            return duration.Enabled ? duration : Vector3.Distance(start, end) / speed;
        }
    }
}