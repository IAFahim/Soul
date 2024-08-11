using _Root.Scripts.Controller.Runtime.Components;
using _Root.Scripts.Model.Runtime.Damages;
using _Root.Scripts.Model.Runtime.Modifiers;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Attacks
{
    [RequireComponent(typeof(BaseReferenceComponent))]
    public class BaseBullet : Attack, ILoadComponent
    {
        public float returnToPoolTime = 5;
        public Modifier damageModifier;
        public float criticalHitChance;
        public float criticalHitMultiplier = 1.5f;
        public float speed = 1f;
        public DelayHandle delayHandle;
        
        public BaseReferenceComponent baseReferenceComponent;

        public override void Execute(AttackIntention attackIntention)
        {
            base.Execute(attackIntention);
            delayHandle = App.Delay(returnToPoolTime, OnComplete);
        }

        private void OnComplete()
        {
            baseReferenceComponent.ReturnToPoolOrDestroy();
        }

        private void MoveAtDirection()
        {
            transform.localPosition += intention.direction * (speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                var damage = damageModifier.ApplyChanceMultiplier(criticalHitChance, criticalHitMultiplier);
                damageable.TakeDamage(damage);
                delayHandle?.Cancel();
                OnComplete();
            }
        }

        private void Update()
        {
            MoveAtDirection();
        }

        void ILoadComponent.OnLoadComponents()
        {
            Reset();
        }

        private void Reset()
        {
            baseReferenceComponent = GetComponent<BaseReferenceComponent>();
        }
    }
}