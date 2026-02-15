using Asteroids.Scripts.Ecs.Movement.Components;
using Asteroids.Scripts.Movement;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Ecs.Movement.Views
{
    public class ProjectileMovementView : MovementBase
    {
        private int _entityId;
        private EcsPool<VelocityResultComponent> _velocityResultPool;

        public void Initialize(EcsWorld world, int entityId)
        {
            _entityId = entityId;
            _velocityResultPool = world.GetPool<VelocityResultComponent>();
        }

        private void FixedUpdate()
        {
            if (_velocityResultPool == null) return;
            VelocityResultComponent result = _velocityResultPool.Get(_entityId);
            ApplyVelocity(result.Velocity);
        }
    }
}