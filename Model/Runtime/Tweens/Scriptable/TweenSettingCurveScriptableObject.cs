using LitMotion;
using UnityEngine;

namespace Soul.Model.Runtime.Tweens.Scriptable
{
    [CreateAssetMenu(fileName = "TweenSettingCurveSO", menuName = "Soul/Tween/TweenSettingCurveSO", order = 1)]
    public class TweenSettingCurveScriptableObject<T> : TweenSettingScriptableObject<T>
    {
        public LoopType loopType;
        public int loopCount;
        public AnimationCurve animationCurve;
    }
}