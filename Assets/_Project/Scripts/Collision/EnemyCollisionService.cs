using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Weapons.Projectile;

namespace Asteroids.Scripts.Collision
{
    public class EnemyCollisionService : CollisionService
    {
        public override bool CanDestroy(IDamageable target)
        {
            return target is IPlayerController;
        }

        public override bool ShouldTakeDamageOnHit(IDamageable self)
        {
            return self is IProjectile;
        }
    }
}