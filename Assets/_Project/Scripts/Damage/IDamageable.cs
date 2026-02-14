using Asteroids.Scripts.Ecs;

namespace Asteroids.Scripts.Damage
{
    public interface IDamageable : IEcsEntity 
    {
        public void TakeDamage(DamageInfo damageInfo);
    }
}