namespace _Project.Scripts.Weapons.Core
{
    public interface IWeapon
    {
        public bool CanShoot { get; }
        public void Shoot();
        public void Recharge(float deltaTime);
    }
}