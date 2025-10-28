using Zenject;

namespace _Project.Scripts.Weapons.Core
{
    public class WeaponUpdater : IWeaponUpdater, ITickable
    {
        private readonly IWeapon[] _weapons;

        [Inject]
        public WeaponUpdater([Inject(Id = "PlayerWeapons")] IWeapon[] weapons)
        {
            _weapons = weapons;
        }

        public void Tick()
        {
            Update(UnityEngine.Time.deltaTime);
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