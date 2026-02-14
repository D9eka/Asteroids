using System;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Views;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.GameState;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player.Input;
using Asteroids.Scripts.Player.Movement;
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
        private readonly PlayerInputHandler _playerInputHandler;
        private readonly PlayerWeaponsInitializer _weaponsInitializer;
        private EcsWorld _ecsWorld;

        public PlayerControllerInitializer(DiContainer container, IAddressableLoader addressableLoader, 
            [Inject(Id = Vector2InjectId.PlayerStartPos)] Vector2 playerSpawnPosition, 
            [Inject(Id = CollisionServiceInjectId.Player)] ICollisionService collisionService,
            IPlayerConfigProvider playerConfigProvider, IEnemyMovementConfigurator enemyMovementConfigurator, 
            IGameStateController gameStateController, IBoundsManager boundsManager, IPauseSystem pauseSystem, 
            IGameplaySessionManager gameplaySessionManager, IPlayerParamsService playerParamsService, 
            PlayerInputHandler playerInputHandler, PlayerWeaponsInitializer weaponsInitializer,
            EcsWorld ecsWorld)
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
            _playerInputHandler = playerInputHandler;
            _weaponsInitializer = weaponsInitializer;
            _ecsWorld = ecsWorld;
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
            
                playerController.Initialize(new PlayerWeaponsHandler(playerWeapons));
                _enemyMovementConfigurator.Initialize(playerGo.transform);
                _gameStateController.Initialize(playerController);
                _boundsManager.RegisterObject(playerGo.transform);
                _gameplaySessionManager.Initialize(playerController);
                _playerParamsService.Initialize(
                    playerGo.transform, playerGo.GetComponent<Rigidbody2D>(), laserGun);
                _playerInputHandler.Initialize(playerController);
                _weaponsInitializer.Initialize(
                    playerGo, _collisionService, playerWeapons, laserGun.GetComponentInChildren<ILineRenderer>());

                InstallEcs(playerGo);
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
        
        private void InstallEcs(GameObject playerGo)
        {
            int playerEntity = _ecsWorld.NewEntity();
            EcsPool<PlayerInputComponent> inputPool = _ecsWorld.GetPool<PlayerInputComponent>();
            inputPool.Add(playerEntity);
            EcsPool<MovementStatsComponent> movementStatsPool = _ecsWorld.GetPool<MovementStatsComponent>();
            movementStatsPool.Add(playerEntity);
            FillMovementStats(ref movementStatsPool.Get(playerEntity));
            EcsPool<TransformDataComponent> transformDataPool = _ecsWorld.GetPool<TransformDataComponent>();
            transformDataPool.Add(playerEntity);
            EcsPool<MovementResultComponent> movementResultPool = _ecsWorld.GetPool<MovementResultComponent>();
            movementResultPool.Add(playerEntity);
            PlayerMovementView playerMovementView = playerGo.GetComponent<PlayerMovementView>();
            playerMovementView.Initialize(_ecsWorld, playerEntity);
            _pauseSystem.Register(playerMovementView);
        }
        private void FillMovementStats(ref MovementStatsComponent stats)
        {
            stats.ThrustForce = _playerConfigProvider.PlayerConfig.MovementConfig.ThrustForce;
            stats.RotationSpeed =  _playerConfigProvider.PlayerConfig.MovementConfig.RotationSpeed;
        }
    }
}