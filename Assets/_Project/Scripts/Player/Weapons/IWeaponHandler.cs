using System.Collections.Generic;
using Asteroids.Scripts.Weapons.Core;

namespace Asteroids.Scripts.Player.Weapons
{
    public interface IWeaponHandler
    {
        public IWeapon CurrentWeapon { get; }

        public void Initialize(IEnumerable<IWeapon> weapons);
        
        public void SwitchWeapon();
    }
}