using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Configs.Snapshot.Player;
using Asteroids.Scripts.Ecs.Components;
using Leopotam.EcsLite;

namespace Asteroids.Scripts.Ecs.Systems
{
    public class ConfigSyncSystem : IEcsInitSystem
    {
        private readonly IPlayerConfigProvider _playerConfigProvider;

        public ConfigSyncSystem(IPlayerConfigProvider playerConfigProvider)
        {
            _playerConfigProvider = playerConfigProvider;
        }
        
        private EcsFilter _playerFilter;
        private EcsPool<PlayerMovementStatsComponent> _movementStatsPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playerFilter = world.Filter<PlayerInputComponent>()
                .Inc<PlayerTransformDataComponent>()
                .End();
            _movementStatsPool = world.GetPool<PlayerMovementStatsComponent>();
            
            _playerConfigProvider.OnConfigUpdated += PlayerConfigProviderOnConfigUpdated;
        }
        private void PlayerConfigProviderOnConfigUpdated()
        {
            foreach (int entity in _playerFilter)
            {
                ref PlayerMovementStatsComponent playerMovementStatsComponent = ref _movementStatsPool.Get(entity);
                PlayerMovementConfig config = _playerConfigProvider.PlayerConfig.MovementConfig;
                playerMovementStatsComponent.ThrustForce = config.ThrustForce;
                playerMovementStatsComponent.RotationSpeed = config.RotationSpeed;
            }
        }
    }
}