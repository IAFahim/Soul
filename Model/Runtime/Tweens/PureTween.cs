using _Root.Scripts.Model.Runtime.Containers;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Tweens
{
    public static class PureTween
    {
        public static MotionHandle PlayMoveWithRotate(this Transform transform, PositionRotation start, PositionRotation end,
            float duration, Ease ease)
        {
            LMotion.Create(start.rotation, end.rotation, duration)
                .WithEase(ease)
                .BindToRotation(transform);
            return LMotion.Create(start.position, end.position, duration)
                .WithEase(ease)
                .BindToPosition(transform);
        }
    }
}