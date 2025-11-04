using System.Collections.Generic;
using Asteroids.Scripts.Core;
using Asteroids.Scripts.Weapons.Core;
using Zenject;

namespace Asteroids.Scripts.Player.Weapons
{
    public class PlayerWeaponsHandler : IWeaponHandler
    {
        private readonly List<IWeapon> _weapons = new();
        private int _currentIndex;
        
        [Inject]
        public PlayerWeaponsHandler([Inject(Id = InjectId.PlayerWeapons)] IWeapon[] weapons)
        {
            _weapons.Clear();
            _weapons.AddRange(weapons);
        }

        public IWeapon CurrentWeapon => _weapons.Count > 0 ? _weapons[_currentIndex] : null;

        public void SwitchWeapon()
        {
            if (_weapons.Count <= 1) return;
            _currentIndex = (_currentIndex + 1) % _weapons.Count;
        }
    }
}