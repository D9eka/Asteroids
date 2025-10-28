using _Project.Scripts.Core;
using _Project.Scripts.Enemies;
using _Project.Scripts.Player;
using _Project.Scripts.Weapons.Projectile;

namespace _Project.Scripts.Collision
{
    public class PlayerCollisionService : CollisionService
    {
        public override bool CanDestroy(IDestroyable target)
        {
            return target is IEnemy;
        }

        public override bool NeedToDestroySelf(IDestroyable self)
        {
            return self is IProjectile or IPlayerController;
        }
    }
}