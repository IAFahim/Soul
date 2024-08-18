using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Soul.Controller.Runtime.Tweens
{
    public static class Tween
    {
        public static MotionHandle JumpOut(Transform targetTransform, float height, float duration, Ease ease)
        {
            var position = targetTransform.position;
            return LMotion.Create(position, new Vector3(position.x, height, position.z), duration)
                .WithEase(ease)
                .BindToPosition(targetTransform);
        }
    }
}