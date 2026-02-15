using System;
using Asteroids.Scripts.Ecs.Player.Components;
using Asteroids.Scripts.Ecs.Weapons.Components;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Ecs.Weapons.Systems
{
    public class PlayerWeaponWantsToShootSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playersFilter;
        private EcsFilter _activeWeaponsFilter;
        private EcsPool<PlayerInputComponent> _playerInputsPool;
        private EcsPool<WeaponOwnerComponent> _weaponOwnersPool;
        private EcsPool<WantsToShootTag> _wantToShootTagsPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playersFilter = world.Filter<PlayerInputComponent>().End();
            _activeWeaponsFilter = world.Filter<ActiveWeaponTag>().End();
            _playerInputsPool = world.GetPool<PlayerInputComponent>();
            _weaponOwnersPool = world.GetPool<WeaponOwnerComponent>();
            _wantToShootTagsPool = world.GetPool<WantsToShootTag>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playersFilter)
            {
                ref PlayerInputComponent playerInputComponent = ref _playerInputsPool.Get(entity);
                int activeWeaponEntity = GetActiveWeaponEntity(entity);
                if (playerInputComponent.IsFiring && !_wantToShootTagsPool.Has(activeWeaponEntity))
                {
                    _wantToShootTagsPool.Add(activeWeaponEntity);
                }
                if (!playerInputComponent.IsFiring && _wantToShootTagsPool.Has(activeWeaponEntity))
                {
                    _wantToShootTagsPool.Del(activeWeaponEntity);
                }
            }
        }

        private int GetActiveWeaponEntity(int playerEntity)
        {
            foreach (int weaponEntity in _activeWeaponsFilter)
            {
                if (_weaponOwnersPool.Get(weaponEntity).OwnerEntity == playerEntity)
                {
                    return weaponEntity;
                }
            }
            throw new NullReferenceException($"Player {playerEntity} dont have active weapon!");
        }
    }
}