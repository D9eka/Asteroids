using Asteroids.Scripts.Ecs.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Ecs.Systems
{
    public class TargetDirectionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _targetDirectionFilter;
        private EcsPool<TargetFollowComponent> _targetFollowPool;
        private EcsPool<LinearDirectionComponent> _linearDirectionPool;
        private EcsPool<PositionComponent> _positionPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _targetDirectionFilter = world.Filter<TargetFollowComponent>()
                .Inc<LinearDirectionComponent>()
                .Inc<PositionComponent>()
                .End();
            _targetFollowPool = world.GetPool<TargetFollowComponent>();
            _linearDirectionPool = world.GetPool<LinearDirectionComponent>();
            _positionPool = world.GetPool<PositionComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _targetDirectionFilter)
            {
                ref TargetFollowComponent targetFollowComponent = ref _targetFollowPool.Get(entity);
                ref LinearDirectionComponent linearDirectionComponent = ref _linearDirectionPool.Get(entity);
                ref PositionComponent positionComponent = ref _positionPool.Get(entity);
                PositionComponent targetPositionComponent = _positionPool.Get(targetFollowComponent.TargetEntity);
                
                if (Time.time - targetFollowComponent.LastUpdateTime >= targetFollowComponent.UpdateInterval)
                {
                    linearDirectionComponent.Direction = (targetPositionComponent.Position - positionComponent.Position).normalized;
                    targetFollowComponent.LastUpdateTime = Time.time;
                }
            }
        }
    }
}