using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Colliders.Components;
using Asteroids.Scripts.Ecs.Movement.Components;
using Asteroids.Scripts.Ecs.Warp.Components;
using Asteroids.Scripts.Ecs.Warp.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Warp.Systems
{
    public class SpawnProtectionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IBoundsWarp _boundsWarp;
        private readonly float _boundsMargin;

        private EcsFilter _protectedFilter;
        private EcsPool<SpawnProtectionComponent> _spawnProtectionPool;
        private EcsPool<PositionComponent> _positionPool;
        private EcsPool<DestroyRequestComponent> _destroyRequestPool;

        public SpawnProtectionSystem(IBoundsWarp boundsWarp)
        {
            _boundsWarp = boundsWarp;
            _boundsMargin = boundsWarp.BoundsMargin;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _protectedFilter = world.Filter<SpawnProtectionComponent>().Inc<PositionComponent>().End();
            _spawnProtectionPool = world.GetPool<SpawnProtectionComponent>();
            _positionPool = world.GetPool<PositionComponent>();
            _destroyRequestPool = world.GetPool<DestroyRequestComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _protectedFilter)
            {
                ref SpawnProtectionComponent protection = ref _spawnProtectionPool.Get(entity);
                protection.RemainingTime -= Time.fixedDeltaTime;

                Vector2 pos = _positionPool.Get(entity).Position;

                if (!IsOutOfBounds(pos))
                {
                    _spawnProtectionPool.Del(entity);
                    continue;
                }

                if (protection.RemainingTime <= 0)
                {
                    _spawnProtectionPool.Del(entity);
                    if (!_destroyRequestPool.Has(entity))
                    {
                        ref DestroyRequestComponent destroyRequest = ref _destroyRequestPool.Add(entity);
                        destroyRequest.DamageType = DamageType.Timeout;
                        destroyRequest.KillerEntity = entity;
                    }
                }
            }
        }

        private bool IsOutOfBounds(Vector2 pos)
        {
            return pos.x < _boundsWarp.MinBounds.x - _boundsMargin ||
                   pos.x > _boundsWarp.MaxBounds.x + _boundsMargin ||
                   pos.y < _boundsWarp.MinBounds.y - _boundsMargin ||
                   pos.y > _boundsWarp.MaxBounds.y + _boundsMargin;
        }
    }
}
