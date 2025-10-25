namespace _Project.Scripts.Weapons
{
    public interface IWeapon
    {
        bool CanShoot { get; }
        void Shoot();
        void Recharge();
    }
}