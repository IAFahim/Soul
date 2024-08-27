using LitMotion;
using UnityEngine;

namespace Soul.Model.Runtime.Tweens
{
    [CreateAssetMenu(fileName = "TweenSettingCurveSO", menuName = "Soul/Tween/TweenSettingCurveSO", order = 1)]
    public class TweenSettingCurveSO<T> : TweenSettingSO<T>
    {
        public LoopType loopType;
        public int loopCount;
        public AnimationCurve animationCurve;
    }
}