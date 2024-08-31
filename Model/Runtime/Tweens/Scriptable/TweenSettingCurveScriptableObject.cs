using LitMotion;
using Soul.Model.Runtime.Tweens.Scriptable;
using UnityEngine;

namespace Soul.Model.Runtime.Tweens
{
    [CreateAssetMenu(fileName = "TweenSettingCurveSO", menuName = "Soul/Tween/TweenSettingCurveSO", order = 1)]
    public class TweenSettingCurveScriptableObject<T> : TweenSettingScriptableObject<T>
    {
        public LoopType loopType;
        public int loopCount;
        public AnimationCurve animationCurve;
    }
}