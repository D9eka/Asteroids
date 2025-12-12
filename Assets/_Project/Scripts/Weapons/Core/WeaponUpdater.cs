using System.Collections.Generic;
using Zenject;

namespace Asteroids.Scripts.Weapons.Core
{
    public class WeaponUpdater : IWeaponUpdater, ITickable
    {
        private readonly List<IWeapon> _weapons = new List<IWeapon>();

        public void Tick()
        {
            Update(UnityEngine.Time.deltaTime);
        }

        public void AddWeapon(IWeapon weapon)
        {
            _weapons.Add(weapon);
        }

        public void Update(float delta)
        {
            foreach (var weapon in _weapons)
            {
                weapon.Recharge(delta);
            }
        }
    }
}