using _Project.Scripts.Weapons.Core;

namespace _Project.Scripts.Weapons.Types.Laser
{
    public interface ILaserGun : IWeapon
    {
        public int CurrentCharges { get; }
        public float ShootCooldown { get; }
    }
}