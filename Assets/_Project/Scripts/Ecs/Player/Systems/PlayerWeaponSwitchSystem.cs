using Asteroids.Scripts.Ecs.Player.Components;
using Asteroids.Scripts.Ecs.Weapons.Components;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Ecs.Weapons.Systems
{
    public class PlayerWeaponSwitchSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playersFilter;
        private EcsFilter _weaponsFilter;
        private EcsPool<PlayerInputComponent> _playerInputsPool;
        private EcsPool<WeaponOwnerComponent> _weaponOwnersPool;
        private EcsPool<ActiveWeaponTag> _activeWeaponsPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playersFilter = world.Filter<PlayerInputComponent>().End();
            _weaponsFilter = world.Filter<WeaponOwnerComponent>().End();
            _playerInputsPool = world.GetPool<PlayerInputComponent>();
            _weaponOwnersPool = world.GetPool<WeaponOwnerComponent>();
            _activeWeaponsPool = world.GetPool<ActiveWeaponTag>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playersFilter)
            {
                ref PlayerInputComponent playerInputComponent = ref _playerInputsPool.Get(entity);
                if (!playerInputComponent.SwitchWeapon) continue;

                int activeWeaponEntity = -1;
                int firstWeapon = -1;
                int nextActiveWeaponEntity = -1;
                foreach (int weaponEntity in _weaponsFilter)
                {
                    if (_weaponOwnersPool.Get(weaponEntity).OwnerEntity != entity) continue;
                    if (firstWeapon == -1) firstWeapon = weaponEntity;
                    if (activeWeaponEntity == -1 && _activeWeaponsPool.Has(weaponEntity))
                    {
                        activeWeaponEntity = weaponEntity;
                        _activeWeaponsPool.Del(weaponEntity);
                    }
                    if (nextActiveWeaponEntity == -1 && weaponEntity != activeWeaponEntity && 
                        !_activeWeaponsPool.Has(weaponEntity))
                    {
                        nextActiveWeaponEntity = weaponEntity;
                        _activeWeaponsPool.Add(weaponEntity);
                    }
                }
                if (nextActiveWeaponEntity == -1)
                {
                    nextActiveWeaponEntity = firstWeapon;
                    _activeWeaponsPool.Add(nextActiveWeaponEntity);
                }
                playerInputComponent.SwitchWeapon = false;
            }
        }
    }
}