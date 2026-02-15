using Asteroids.Scripts.Ecs.Colliders.Components;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Ecs.Collision.Systems
{
    public class DamageSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _damageEventFilter;
        private EcsPool<DamageEventComponent> _damageEventPool;
        private EcsPool<DestroyOnHitTag> _destroyOnHitTagPool;
        private EcsPool<DestroyRequestComponent> _destroyRequestPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _damageEventFilter = world.Filter<DamageEventComponent>().End();
            _damageEventPool = world.GetPool<DamageEventComponent>();
            _destroyOnHitTagPool = world.GetPool<DestroyOnHitTag>();
            _destroyRequestPool = world.GetPool<DestroyRequestComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _damageEventFilter)
            {
                if (_destroyOnHitTagPool.Has(entity) && !_destroyRequestPool.Has(entity))
                {
                    DamageEventComponent damageEventComponent = _damageEventPool.Get(entity);
                    ref DestroyRequestComponent destroyRequestComponent = ref _destroyRequestPool.Add(entity);
                    destroyRequestComponent.DamageType = damageEventComponent.DamageType;
                    destroyRequestComponent.KillerEntity = damageEventComponent.SourceEntity;
                }
                _damageEventPool.Del(entity);
            }
        }
    }
}