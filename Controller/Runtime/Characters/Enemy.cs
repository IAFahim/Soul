using _Root.Scripts.Model.Runtime.Damages;
using _Root.Scripts.Model.Runtime.Enemies;
using _Root.Scripts.Model.Runtime.Stats;

namespace _Root.Scripts.Controller.Runtime.Characters
{
    public class Enemy : CharacterComponent, IHealthReference, IDamageable
    {
        public Health health;
        public EnemyType type;
        public Health Health => health;
        
        public float TakeDamage(float damage) => health.TakeDamage(damage);
    }
}