using Soul.Model.Runtime.Damages;
using Soul.Model.Runtime.Enemies;
using Soul.Model.Runtime.Stats;

namespace Soul.Controller.Runtime.Characters
{
    public class Enemy : CharacterComponent, IHealthReference, IDamageable
    {
        public Health health;
        public EnemyType type;
        public Health Health => health;
        
        public float TakeDamage(float damage) => health.TakeDamage(damage);
    }
}