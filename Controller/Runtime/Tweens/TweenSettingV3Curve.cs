using Soul.Model.Runtime.Tweens;
using Soul.Model.Runtime.Tweens.Scriptable;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Soul.Controller.Runtime.Tweens
{
    [CreateAssetMenu(fileName = "TweenSettingV3CurveSO", menuName = "Soul/Tween/TweenSettingV3CurveSO", order = 1)]
    public class TweenSettingV3Curve : TweenSettingCurveScriptableObject<Vector3>
    {
    }
}