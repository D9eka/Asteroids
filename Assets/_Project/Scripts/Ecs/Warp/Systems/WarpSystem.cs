using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Colliders.Components;
using Asteroids.Scripts.Ecs.Movement.Components;
using Asteroids.Scripts.Ecs.Warp.Components;
using Asteroids.Scripts.Ecs.Warp.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Warp.Systems
{
    public class WarpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IBoundsWarp _boundsWarp;
        private readonly float _boundsMargin;

        private EcsFilter _warpableFilter;
        private EcsFilter _destroyOnOutOfBoundsFilter;
        private EcsFilter _warpEventFilter;
        private EcsPool<PositionComponent> _positionPool;
        private EcsPool<WarpEventComponent> _warpEventPool;
        private EcsPool<DestroyRequestComponent> _destroyRequestPool;

        public WarpSystem(IBoundsWarp boundsWarp)
        {
            _boundsWarp = boundsWarp;
            _boundsMargin = boundsWarp.BoundsMargin;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _warpableFilter = world.Filter<PositionComponent>().Inc<WarpTag>()
                .Exc<SpawnProtectionComponent>().End();
            _destroyOnOutOfBoundsFilter = world.Filter<PositionComponent>()
                .Inc<DestroyOnOutOfBoundsTag>().Exc<SpawnProtectionComponent>().End();
            _warpEventFilter = world.Filter<WarpEventComponent>().End();

            _positionPool = world.GetPool<PositionComponent>();
            _warpEventPool = world.GetPool<WarpEventComponent>();
            _destroyRequestPool = world.GetPool<DestroyRequestComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _warpEventFilter)
            {
                _warpEventPool.Del(entity);
            }

            foreach (int entity in _warpableFilter)
            {
                Vector2 pos = _positionPool.Get(entity).Position;
                if (!IsOutOfBounds(pos)) continue;

                Vector2 newPos = CalculateWarpPosition(pos);
                ref PositionComponent posComp = ref _positionPool.Get(entity);
                posComp.Position = newPos;

                ref WarpEventComponent warpEvent = ref _warpEventPool.Add(entity);
                warpEvent.NewPosition = newPos;
            }

            foreach (int entity in _destroyOnOutOfBoundsFilter)
            {
                Vector2 pos = _positionPool.Get(entity).Position;
                if (!IsOutOfBounds(pos)) continue;

                if (!_destroyRequestPool.Has(entity))
                {
                    ref DestroyRequestComponent destroyRequest = ref _destroyRequestPool.Add(entity);
                    destroyRequest.DamageType = DamageType.OutOfBounds;
                    destroyRequest.KillerEntity = entity;
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

        private Vector2 CalculateWarpPosition(Vector2 pos)
        {
            if (pos.x < _boundsWarp.MinBounds.x - _boundsMargin)
                pos.x = _boundsWarp.MaxBounds.x + _boundsMargin;
            else if (pos.x > _boundsWarp.MaxBounds.x + _boundsMargin)
                pos.x = _boundsWarp.MinBounds.x - _boundsMargin;

            if (pos.y < _boundsWarp.MinBounds.y - _boundsMargin)
                pos.y = _boundsWarp.MaxBounds.y + _boundsMargin;
            else if (pos.y > _boundsWarp.MaxBounds.y + _boundsMargin)
                pos.y = _boundsWarp.MinBounds.y - _boundsMargin;

            return pos;
        }
    }
}
