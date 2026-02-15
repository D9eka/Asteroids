using System;
using Asteroids.Scripts.Core;

namespace Asteroids.Scripts.Weapons.Core
{
    public interface IWeapon : ITransformProvider
    {
        public event Action<IWeapon> OnShoot;
        
        public void Shoot();
    }
}