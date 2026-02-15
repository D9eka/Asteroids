using System;
using Asteroids.Scripts.Ecs.Components;
using Asteroids.Scripts.Ecs.Views;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Colliders.Components;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Asteroids.Scripts.Ecs.Weapons.Components;
using Asteroids.Scripts.GameState;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player.Weapons;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.WarpSystem;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;
using Asteroids.Scripts.Weapons.Types.Laser.LineRenderer;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Player
{
    public class PlayerControllerInitializer : IInitializable
    {
        private readonly DiContainer _container;
        private readonly IAddressableLoader _addressableLoader;
        private readonly Vector2 _playerSpawnPosition;
        private readonly ICollisionService _collisionService;
        private readonly IPlayerConfigProvider _playerConfigProvider;
        private readonly IEnemyMovementConfigurator _enemyMovementConfigurator;
        private readonly IGameStateController _gameStateController;
        private readonly IBoundsManager _boundsManager;
        private readonly IPauseSystem _pauseSystem;
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly IPlayerParamsService _playerParamsService;
        private readonly PlayerWeaponsInitializer _weaponsInitializer;
        private readonly EcsWorld _ecsWorld;
        private readonly EntityViewRegistry _entityViewRegistry;

        public PlayerControllerInitializer(DiContainer container, IAddressableLoader addressableLoader, 
            [Inject(Id = Vector2InjectId.PlayerStartPos)] Vector2 playerSpawnPosition, 
            PlayerCollisionService collisionService,
            IPlayerConfigProvider playerConfigProvider, IEnemyMovementConfigurator enemyMovementConfigurator, 
            IGameStateController gameStateController, IBoundsManager boundsManager, IPauseSystem pauseSystem, 
            IGameplaySessionManager gameplaySessionManager, IPlayerParamsService playerParamsService, 
            PlayerWeaponsInitializer weaponsInitializer, EcsWorld ecsWorld, EntityViewRegistry entityViewRegistry)
        {
            _container = container;
            _addressableLoader = addressableLoader;
            _playerSpawnPosition = playerSpawnPosition;
            _collisionService = collisionService;
            _playerConfigProvider = playerConfigProvider;
            _enemyMovementConfigurator = enemyMovementConfigurator;
            _gameStateController = gameStateController;
            _boundsManager = boundsManager;
            _pauseSystem = pauseSystem;
            _gameplaySessionManager = gameplaySessionManager;
            _playerParamsService = playerParamsService;
            _weaponsInitializer = weaponsInitializer;
            _ecsWorld = ecsWorld;
            _entityViewRegistry = entityViewRegistry;
        }

        public async void Initialize()
        {
            try
            {
                GameObject playerGo = await SpawnPlayer();
                playerGo.transform.position = _playerSpawnPosition;
                
                BulletGun bulletGun = playerGo.GetComponentInChildren<BulletGun>();
                LaserGun laserGun = playerGo.GetComponentInChildren<LaserGun>();
                IWeapon[] playerWeapons = { bulletGun, laserGun };
            
                PlayerController playerController = playerGo.GetComponent<PlayerController>();
                playerController.GetComponent<CollisionHandler>().Initialize(_collisionService);
                InstallEcs(playerGo, playerController);
                _weaponsInitializer.Initialize(playerController, _collisionService, playerWeapons, 
                    laserGun.GetComponentInChildren<ILineRenderer>());
                
                _gameStateController.Initialize(playerController);
                _boundsManager.RegisterObject(playerGo.transform);
                _gameplaySessionManager.Initialize(playerController);
                _playerParamsService.Initialize(
                    playerGo.transform, playerGo.GetComponent<Rigidbody2D>(),
                    _ecsWorld.GetPool<LaserGunComponent>().Get(laserGun.Id));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async UniTask<GameObject> SpawnPlayer()
        {
            GameObject playerPrefab = await _addressableLoader.Load<GameObject>(AddressableId.Player);
            var playerGo = _container.InstantiatePrefab(playerPrefab);
            return playerGo;
        }
        
        private void InstallEcs(GameObject playerGo, PlayerController playerController)
        {
            int playerEntity = _ecsWorld.NewEntity();
            playerController.SetId(playerEntity);
            _ecsWorld.GetPool<PositionComponent>().Add(playerEntity);
            _ecsWorld.GetPool<PlayerInputComponent>().Add(playerEntity);
            EcsPool<PlayerMovementStatsComponent> movementStatsPool = _ecsWorld.GetPool<PlayerMovementStatsComponent>();
            movementStatsPool.Add(playerEntity);
            FillMovementStats(ref movementStatsPool.Get(playerEntity));
            EcsPool<PlayerTransformDataComponent> transformDataPool = _ecsWorld.GetPool<PlayerTransformDataComponent>();
            transformDataPool.Add(playerEntity);
            EcsPool<PlayerMovementResultComponent> movementResultPool = _ecsWorld.GetPool<PlayerMovementResultComponent>();
            movementResultPool.Add(playerEntity);
            PlayerMovementView playerMovementView = playerGo.GetComponent<PlayerMovementView>();
            playerMovementView.Initialize(_ecsWorld, playerEntity);
            _pauseSystem.Register(playerMovementView);
            _enemyMovementConfigurator.Initialize(playerEntity);
            _ecsWorld.GetPool<PlayerTag>().Add(playerEntity);
            _ecsWorld.GetPool<DestroyOnHitTag>().Add(playerEntity);
            EcsPool<DamageSourceComponent> damageSourcesPool = _ecsWorld.GetPool<DamageSourceComponent>();
            ref DamageSourceComponent damageSourceComponent = ref damageSourcesPool.Add(playerEntity);
            damageSourceComponent.Type = DamageType.Collide;
            _entityViewRegistry.Register(playerEntity, playerController);
        }
        private void FillMovementStats(ref PlayerMovementStatsComponent stats)
        {
            stats.ThrustForce = _playerConfigProvider.PlayerConfig.MovementConfig.ThrustForce;
            stats.RotationSpeed =  _playerConfigProvider.PlayerConfig.MovementConfig.RotationSpeed;
        }
    }
}