using Asteroids.Scripts.Weapons.Core;

namespace Asteroids.Scripts.Weapons.Types.Laser
{
    public interface ILaserGun : IWeapon
    {
        public int CurrentCharges { get; }
        public float ShootCooldown { get; }
    }
}