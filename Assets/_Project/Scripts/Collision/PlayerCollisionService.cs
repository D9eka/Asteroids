using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Weapons.Projectile;

namespace Asteroids.Scripts.Collision
{
    public class PlayerCollisionService : CollisionService
    {
        public override bool CanDestroy(IDamageable target)
        {
            return target is IEnemy;
        }

        public override bool ShouldTakeDamageOnHit(IDamageable self)
        {
            return self is IProjectile;
        }
    }
}