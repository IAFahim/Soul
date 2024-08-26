using System;
using LitMotion;
using UnityEngine;

namespace Soul.Model.Runtime.Tweens
{
    [Serializable]
    public struct TweenSettingsV3Ya
    {
        public Vector3 start;
        public Vector3 end;
        public float duration;
        public LoopType loopType;
        public int loopCount;
        public AnimationCurve animationCurve;
    }
}