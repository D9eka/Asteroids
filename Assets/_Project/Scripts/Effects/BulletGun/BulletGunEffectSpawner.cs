using System;
using System.Collections.Generic;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Types.BulletGun;

namespace Asteroids.Scripts.Effects
{
    public class BulletGunEffectSpawner : IDisposable
    {
        private readonly BulletGunEffectFactory _factory;
        private readonly List<BulletGun> _weapons = new List<BulletGun>();

        public BulletGunEffectSpawner(BulletGunEffectFactory factory)
        {
            _factory = factory;
        }
        
        public void AddWeapon(BulletGun weapon)
        {
            _weapons.Add(weapon);
            weapon.OnShoot += PlayShootSound;
        }

        public void Dispose()
        {
            foreach (BulletGun weapon in _weapons)
            {
                weapon.OnShoot -= PlayShootSound;
            }
        }
        
        private void PlayShootSound(IWeapon weapon)
        {
            _factory.Create(weapon.Transform);
        }
    }
}