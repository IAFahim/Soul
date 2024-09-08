using System;
using LitMotion;
using Soul.Model.Runtime.Effects;
using UnityEngine;

namespace Soul.Controller.Runtime.Effects
{
    [Serializable]
    public class FreezeEffect : Effect
    {
        public float damage = 10;
        public override float EffectStrength => damage;
        public override float Duration => baseDuration;

        public override void Apply(IEffectConsumer effectConsumer)
        {
            Debug.Log("Freeze effect applied to " + effectConsumer);
            EffectMotionHandle = LMotion.Create(0, 1, Duration).WithOnCancel(Cancel).WithOnComplete(OnComplete).RunWithoutBinding();
        }

        public override void OnComplete()
        {
            base.OnComplete();
            Debug.Log("Freeze effect completed");
        }
    }
}