using LitMotion;
using LitMotion.Extensions;
using Soul.Model.Runtime.Containers;
using UnityEngine;

namespace Soul.Model.Runtime.Tweens
{
    public static class PureTween
    {
        public static MotionHandle PlayMoveWithRotate(this Transform transform, PositionRotation start,
            PositionRotation end,
            float duration, Ease ease)
        {
            LMotion.Create(start.rotation, end.rotation, duration)
                .WithEase(ease)
                .BindToRotation(transform);
            return LMotion.Create(start.position, end.position, duration)
                .WithEase(ease)
                .BindToPosition(transform);
        }

        public static MotionHandle DualSquishAndStretch(this Transform transform, Vector3 start, Vector3 end,
            float duration, AnimationCurve animationCurve)
        {
            return LMotion.Create(start, end, duration)
                .WithEase(animationCurve)
                .WithLoops(2, LoopType.Yoyo)
                .BindToLocalScale(transform);
        }
    }
}