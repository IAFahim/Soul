using System;
using LitMotion;
using Pancake;
using UnityEngine;

namespace Soul.Model.Runtime.Tweens
{
    [Serializable]
    public abstract class VariableTween
    {
        public Optional<float> duration;
        public float speed;

        public abstract MotionHandle Play(Transform transform);
        public abstract float CalculateDuration();
    }
}