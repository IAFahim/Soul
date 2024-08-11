using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Soul.Model.Runtime.Tweens
{
    [Serializable]
    public class MoveVariableTween : Vector3StartEndVariableTween
    {
        public override MotionHandle Play(Transform transform)
        {
            duration = CalculateDuration();
            if (!start.Enabled) start = transform.position;
            return LMotion.Create(start, end, duration).BindToEulerAngles(transform);
        }
    }
}