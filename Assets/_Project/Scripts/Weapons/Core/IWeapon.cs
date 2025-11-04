using Asteroids.Scripts.Damage;

namespace Asteroids.Scripts.Weapons.Core
{
    public interface IWeapon : IDamageSource
    {
        public void Shoot();
        public void Recharge(float deltaTime);
    }
}