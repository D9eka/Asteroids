using System.Collections.Generic;
using _Project.Scripts.Weapons.Core;

namespace _Project.Scripts.Player.Weapons
{
    public interface IWeaponHandler
    {
        public IWeapon CurrentWeapon { get; }

        public void Initialize(IEnumerable<IWeapon> weapons);
        
        public void SwitchWeapon();
    }
}