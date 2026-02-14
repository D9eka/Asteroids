using _Project.Scripts.Ecs.Components;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Configs.Snapshot.Player;
using Leopotam.EcsLite;

namespace _Project.Scripts.Ecs.Systems
{
    public class ConfigSyncSystem : IEcsInitSystem
    {
        private readonly IPlayerConfigProvider _playerConfigProvider;

        public ConfigSyncSystem(IPlayerConfigProvider playerConfigProvider)
        {
            _playerConfigProvider = playerConfigProvider;
        }
        
        private EcsFilter _playerFilter;
        private EcsPool<MovementStatsComponent> _movementStatsPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playerFilter = world.Filter<PlayerInputComponent>()
                .Inc<TransformDataComponent>()
                .End();
            _movementStatsPool = world.GetPool<MovementStatsComponent>();
            
            _playerConfigProvider.OnConfigUpdated += PlayerConfigProviderOnConfigUpdated;
        }
        private void PlayerConfigProviderOnConfigUpdated()
        {
            foreach (int entity in _playerFilter)
            {
                ref MovementStatsComponent movementStatsComponent = ref _movementStatsPool.Get(entity);
                PlayerMovementConfig config = _playerConfigProvider.PlayerConfig.MovementConfig;
                movementStatsComponent.ThrustForce = config.ThrustForce;
                movementStatsComponent.RotationSpeed = config.RotationSpeed;
            }
        }
    }
}