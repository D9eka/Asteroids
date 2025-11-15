using Asteroids.Scripts.Weapons.Core;

namespace Asteroids.Scripts.Player.Weapons
{
    public interface IWeaponHandler
    {
        public IWeapon CurrentWeapon { get; }
        
        public void SwitchWeapon();
    }
}