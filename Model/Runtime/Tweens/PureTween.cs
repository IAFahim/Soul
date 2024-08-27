using LitMotion;
using LitMotion.Extensions;
using Soul.Model.Runtime.Containers;
using UnityEngine;

namespace Soul.Model.Runtime.Tweens
{
    public static class PureTween
    {
        public static MotionHandle PlayMoveWithRotate(this Transform transform,
            PositionRotation start, PositionRotation end, float duration, Ease ease
        )
        {
            LMotion.Create(start.rotation, end.rotation, duration)
                .WithEase(ease)
                .BindToRotation(transform);
            return LMotion.Create(start.position, end.position, duration)
                .WithEase(ease)
                .BindToPosition(transform);
        }

        public static MotionHandle TweenPlayer(this Transform transform,
            Vector3 start, Vector3 end, float duration, int loopCount, LoopType loopType, AnimationCurve animationCurve
        )
        {
            return LMotion.Create(start, end, duration)
                .WithEase(animationCurve)
                .WithLoops(loopCount, loopType)
                .BindToLocalScale(transform);
        }
        
        public static MotionHandle TweenHeight(Transform targetTransform, float height, float duration, Ease ease)
        {
            var position = targetTransform.position;
            return LMotion.Create(position, new Vector3(position.x, height, position.z), duration)
                .WithEase(ease)
                .BindToPosition(targetTransform);
        }

        public static MotionHandle TweenPlayer(this Transform transform,
            TweenSettingCurveSO<Vector3> settings)
        {
            return TweenPlayer(transform, settings.start, settings.end, settings.duration,
                settings.loopCount, settings.loopType, settings.animationCurve);
        }
        
    }
}