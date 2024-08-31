using UnityEngine;

namespace Soul.Model.Runtime.Tweens.Scriptable
{
    [CreateAssetMenu(fileName = "TweenSettingSO", menuName = "Soul/Tween/TweenSettingSO", order = 0)]
    public class TweenSettingScriptableObject<T> : TweenSettingBaseScriptableObject
    {
        public T start;
        public T end;
    }
}