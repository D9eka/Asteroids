using _Project.Scripts.Collision;
using _Project.Scripts.Damage;
using _Project.Scripts.Pause;
using _Project.Scripts.Spawning.Common.Pooling;

namespace _Project.Scripts.Weapons.Projectile
{
    public interface IProjectile : IDamageSource, IPoolable, IPausable
    {
        public void Initialize(ProjectileData data, DamageInfo damageInfo, ICollisionService collisionService);
    }
}