using _Project.Scripts.Core;
using _Project.Scripts.Damage;
using _Project.Scripts.Enemies;
using _Project.Scripts.Player;
using _Project.Scripts.Weapons.Projectile;

namespace _Project.Scripts.Collision
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