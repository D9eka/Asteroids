using System.Collections.Generic;
using _Project.Scripts.Core.InjectIds;
using Asteroids.Scripts.Core;
using Zenject;

namespace Asteroids.Scripts.Weapons.Core
{
    public class WeaponUpdater : IWeaponUpdater, ITickable
    {
        private readonly List<IWeapon> _weapons = new List<IWeapon>();

        [Inject]
        public WeaponUpdater([Inject(Id = WeaponInjectId.PlayerWeapons)] IWeapon[] weapons)
        {
            _weapons.AddRange(weapons);
        }

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