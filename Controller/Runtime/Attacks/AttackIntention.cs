using System;
using Pancake;
using Soul.Controller.Runtime.Effects;
using UnityEngine;

namespace Soul.Controller.Runtime.Attacks
{
    [Serializable]
    public struct AttackIntention
    {
        [SerializeField] public Transform attacker;
        [SerializeField] public Vector3 direction;
        [SerializeField] public LayerMask layerToEffect;
        [SerializeField] public Optional<EffectTargetComponent> optionalTarget;

        public AttackIntention(Transform attacker, Vector3 direction, LayerMask layerToEffect,
            Optional<EffectTargetComponent> optionalTarget)
        {
            this.attacker = attacker;
            this.direction = direction;
            this.layerToEffect = layerToEffect;
            this.optionalTarget = optionalTarget;
        }

        public AttackIntention(Transform attacker, Vector3 direction, LayerMask layerToEffect)
        {
            this.attacker = attacker;
            this.direction = direction;
            this.layerToEffect = layerToEffect;
            optionalTarget = new Optional<EffectTargetComponent>(false, null);
        }
    }
}