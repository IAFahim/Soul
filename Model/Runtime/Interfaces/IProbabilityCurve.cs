using UnityEngine;

namespace Soul.Model.Runtime.Interfaces
{
    public interface IProbabilityCurve
    {
        AnimationCurve ProbabilityCurve { get; }
    }
}