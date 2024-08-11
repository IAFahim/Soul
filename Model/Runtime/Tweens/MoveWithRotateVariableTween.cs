using System;
using _Root.Scripts.Model.Runtime.Containers;
using LitMotion;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Tweens
{
    [Serializable]
    public class MoveWithRotateVariableTween : VariableTween
    {
        public Optional<PositionRotation> start;
        public PositionRotation end;
        public Ease ease;

        public override MotionHandle Play(Transform transform)
        {
            duration = CalculateDuration();
            if (!start.Enabled)
                start = new Optional<PositionRotation>(new PositionRotation
                {
                    position = transform.position,
                    rotation = transform.rotation
                });

            return transform.PlayMoveWithRotate(start, end, duration, ease);
        }


        public override float CalculateDuration()
        {
            return duration.Enabled ? duration : Vector3.Distance(start.Value, end) / speed;
        }
    }
}