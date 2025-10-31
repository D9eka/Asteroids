using Asteroids.Scripts.Damage;

namespace Asteroids.Scripts.Weapons.Core
{
    public interface IWeapon : IDamageSource
    {
        public bool CanShoot { get; }
        public void Shoot();
        public void Recharge(float deltaTime);
    }
}