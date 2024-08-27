using LitMotion;
using UnityEngine;

namespace Soul.Model.Runtime.Tweens
{
    [CreateAssetMenu(fileName = "TweenSettingCurveSO", menuName = "Soul/Tween/TweenSettingSO", order = 0)]
    public class TweenSettingSO<T> : ScriptableObject
    {
        public T start;
        public T end;
        public float duration;
        public Ease ease;
    }
}