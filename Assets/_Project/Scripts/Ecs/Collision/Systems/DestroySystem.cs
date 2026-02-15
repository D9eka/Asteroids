using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Colliders.Components;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Ecs.Collision.Systems
{
    public class DestroySystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EntityViewRegistry _entityViewRegistry;

        public DestroySystem(EntityViewRegistry entityViewRegistry)
        {
            _entityViewRegistry = entityViewRegistry;
        }

        private EcsFilter _destroyRequestFilter;
        private EcsPool<DestroyRequestComponent> _destroyRequestPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _destroyRequestFilter = world.Filter<DestroyRequestComponent>().End();
            _destroyRequestPool = world.GetPool<DestroyRequestComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _destroyRequestFilter)
            {
                DestroyRequestComponent request = _destroyRequestPool.Get(entity);
                _destroyRequestPool.Del(entity);

                if (_entityViewRegistry.TryGet(entity, out IDamageable damageable))
                {
                    _entityViewRegistry.TryGet(request.KillerEntity, out IDamageable killer);
                    damageable.TakeDamage(new DamageInfo(request.DamageType, killer));
                }
            }
        }
    }
}