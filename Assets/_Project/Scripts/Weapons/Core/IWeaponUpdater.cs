namespace Asteroids.Scripts.Weapons.Core
{
    public interface IWeaponUpdater
    {
        public void AddWeapon(IWeapon weapon);
        
        public void Update(float deltaTime);
    }
}