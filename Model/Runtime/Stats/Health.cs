using System;
using Soul.Model.Runtime.Damages;
using UnityEngine;

namespace Soul.Model.Runtime.Stats
{
    [Serializable]
    public class Health : IDamageable
    {
        [Tooltip("Current health value")] public float currentHealth = 100;
        [Tooltip("Starting health value")] public float initialHealth = 100;
        [Tooltip("Maximum health value")] public float maxHealth = 100;

        [Tooltip("Check this to disable receiving damage")]
        public bool invincible;

        public float TakeDamage(float damage)
        {
            if (invincible) return 0;
            var damageTaken = Mathf.Min(damage, currentHealth);
            currentHealth -= damageTaken;
            if (currentHealth <= 0) Die();
            return damageTaken;
        }

        // [Tooltip("another Health component (usually on another character) towards which all health will be redirected")]
        // public Optional<Health> masterHealth;
        //
        // public bool useMasterHealth;

        private void SetToInitialHealth()
        {
            currentHealth = initialHealth;
        }

        private void Die()
        {
            StopAllDamageOverTime();
        }

        private void StopAllDamageOverTime()
        {
        }

        public static implicit operator float(Health health)
        {
            return health.currentHealth;
        }
    }
}