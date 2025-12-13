using System;
using System.Threading.Tasks;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Collision;
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
        private readonly PlayerMovementData _movementData;
        private readonly IEnemyMovementConfigurator _enemyMovementConfigurator;
        private readonly IGameStateController _gameStateController;
        private readonly IBoundsManager _boundsManager;
        private readonly IPauseSystem _pauseSystem;
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly IPlayerParamsService _playerParamsService;
        private readonly PlayerInputHandler _playerInputHandler;
        private readonly PlayerWeaponsInitializer _weaponsInitializer;

        public PlayerControllerInitializer(DiContainer container, IAddressableLoader addressableLoader, 
            [Inject(Id = Vector2InjectId.PlayerStartPos)] Vector2 playerSpawnPosition, 
            [Inject(Id = CollisionServiceInjectId.Player)] ICollisionService collisionService,
            PlayerMovementData movementData, IEnemyMovementConfigurator enemyMovementConfigurator, 
            IGameStateController gameStateController, IBoundsManager boundsManager, IPauseSystem pauseSystem, 
            IGameplaySessionManager gameplaySessionManager, IPlayerParamsService playerParamsService, 
            PlayerInputHandler playerInputHandler, PlayerWeaponsInitializer weaponsInitializer)
        {
            _container = container;
            _addressableLoader = addressableLoader;
            _playerSpawnPosition = playerSpawnPosition;
            _collisionService = collisionService;
            _movementData = movementData;
            _enemyMovementConfigurator = enemyMovementConfigurator;
            _gameStateController = gameStateController;
            _boundsManager = boundsManager;
            _pauseSystem = pauseSystem;
            _gameplaySessionManager = gameplaySessionManager;
            _playerParamsService = playerParamsService;
            _playerInputHandler = playerInputHandler;
            _weaponsInitializer = weaponsInitializer;
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
                PlayerMovement playerMovement = playerGo.GetComponent<PlayerMovement>();
            
                playerController.GetComponent<CollisionHandler>().Initialize(_collisionService);
                playerMovement.Initialize(_movementData);
            
                playerController.Initialize(playerMovement, new PlayerWeaponsHandler(playerWeapons));
                _enemyMovementConfigurator.Initialize(playerGo.transform);
                _gameStateController.Initialize(playerController);
                _boundsManager.RegisterObject(playerGo.transform);
                _pauseSystem.Register(playerController);
                _gameplaySessionManager.Initialize(playerController);
                _playerParamsService.Initialize(
                    playerGo.transform, playerGo.GetComponent<Rigidbody2D>(), laserGun);
                _playerInputHandler.Initialize(playerController);
                _weaponsInitializer.Initialize(
                    playerGo, _collisionService, playerWeapons, laserGun.GetComponentInChildren<ILineRenderer>());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task<GameObject> SpawnPlayer()
        {
            Task<GameObject> task = _addressableLoader.Load<GameObject>(AddressableId.Player);
            await task;
            var playerGo = _container.InstantiatePrefab(task.Result);
            return playerGo;
        }
    }
}