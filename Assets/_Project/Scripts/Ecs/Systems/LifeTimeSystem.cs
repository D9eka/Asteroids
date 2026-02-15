using System;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Colliders.Components;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Ecs.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Systems
{
    public class LifeTimeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _lifeTimeFilter;
        private EcsPool<LifeTimeComponent> _lifeTimePool;
        private EcsPool<DestroyRequestComponent> _destroyRequestPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _lifeTimeFilter = world.Filter<LifeTimeComponent>().End();
            _lifeTimePool = world.GetPool<LifeTimeComponent>();
            _destroyRequestPool = world.GetPool<DestroyRequestComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _lifeTimeFilter)
            {
                ref LifeTimeComponent lifeTimeComponent = ref _lifeTimePool.Get(entity);
                lifeTimeComponent.RemainingTime -= Time.fixedDeltaTime;
                if (lifeTimeComponent.RemainingTime <= 0)
                {
                    ref DestroyRequestComponent destroyRequestComponent = ref _destroyRequestPool.Add(entity);
                    destroyRequestComponent.DamageType = DamageType.Timeout;
                    destroyRequestComponent.KillerEntity = entity;
                    _lifeTimePool.Del(entity);
                }
            }
        }
    }
}