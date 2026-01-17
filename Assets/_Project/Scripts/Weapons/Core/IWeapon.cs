using System;
using Asteroids.Scripts.Core;
using Asteroids.Scripts.Damage;

namespace Asteroids.Scripts.Weapons.Core
{
    public interface IWeapon : IDamageSource, ITransformProvider
    {
        public event Action<IWeapon> OnShoot;
        
        public void Shoot();
        public void Recharge(float deltaTime);
    }
}