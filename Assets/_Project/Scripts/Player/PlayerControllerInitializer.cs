using System.Collections.Generic;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Runtime;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player.Movement;
using Asteroids.Scripts.Player.Weapons;
using Asteroids.Scripts.Spawning.Enemies.Movement;
using Asteroids.Scripts.WarpSystem;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;
using Asteroids.Scripts.Weapons.Types.Laser.LineRenderer;
using Cysharp.Threading.Tasks;
using Fusion;
using _Project.Scripts.Multiplayer;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Player
{
    public class PlayerControllerInitializer : IInitializable
    {
        private const float SPACING = 2f;
        
        private readonly DiContainer _container;
        private readonly NetworkObject _networkPlayerPrefab;
        private readonly Transform[] _playerSpawnPoints;
        private readonly Vector2 _playerSpawnPosition;
        private readonly ICollisionService _collisionService;
        private readonly IPlayerConfigProvider _playerConfigProvider;
        private readonly IEnemyMovementConfigurator _enemyMovementConfigurator;
        private readonly PlayerControllerRegistry _playerControllerRegistry;
        private readonly IBoundsManager _boundsManager;
        private readonly IPauseSystem _pauseSystem;
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly IPlayerParamsService _playerParamsService;
        private readonly PlayerWeaponsInitializer _weaponsInitializer;
        private readonly NetworkEventsRouter _networkEventsRouter;

        private readonly Dictionary<PlayerRef, NetworkObject> _spawnedPlayers = new();
        private readonly HashSet<PlayerRef> _localInitialized = new();

        public PlayerControllerInitializer(
            DiContainer container,
            NetworkObject networkPlayerPrefab,
            Transform[] playerSpawnPoints,
            [Inject(Id = Vector2InjectId.PlayerStartPos)] Vector2 playerSpawnPosition, 
            [Inject(Id = CollisionServiceInjectId.Player)] ICollisionService collisionService,
            IPlayerConfigProvider playerConfigProvider, IEnemyMovementConfigurator enemyMovementConfigurator, 
            PlayerControllerRegistry playerControllerRegistry, IBoundsManager boundsManager, IPauseSystem pauseSystem, 
            IGameplaySessionManager gameplaySessionManager, IPlayerParamsService playerParamsService, 
            PlayerWeaponsInitializer weaponsInitializer, NetworkEventsRouter networkEventsRouter)
        {
            _container = container;
            _networkPlayerPrefab = networkPlayerPrefab;
            _playerSpawnPoints = playerSpawnPoints;
            _playerSpawnPosition = playerSpawnPosition;
            _collisionService = collisionService;
            _playerConfigProvider = playerConfigProvider;
            _enemyMovementConfigurator = enemyMovementConfigurator;
            _playerControllerRegistry = playerControllerRegistry;
            _boundsManager = boundsManager;
            _pauseSystem = pauseSystem;
            _gameplaySessionManager = gameplaySessionManager;
            _playerParamsService = playerParamsService;
            _weaponsInitializer = weaponsInitializer;
            _networkEventsRouter = networkEventsRouter;
        }

        public void Initialize()
        {
            _networkEventsRouter.PlayerJoinedEvent += OnPlayerJoined;
            _networkEventsRouter.PlayerLeftEvent += OnPlayerLeft;
            _networkEventsRouter.SceneLoadDoneEvent += OnSceneLoadDone;

            NetworkRunner runner = _networkEventsRouter.GetAttachedRunner();
            HandleRunnerReady(runner);
        }

        private void OnSceneLoadDone(NetworkRunner runner)
        {
            HandleRunnerReady(runner);
        }

        private void HandleRunnerReady(NetworkRunner runner)
        {
            if (runner == null)
                return;

            if (runner.IsServer)
            {
                SpawnActivePlayers(runner);
            }

            TryInitializeLocalPlayer(runner, runner.LocalPlayer).Forget();
        }

        private void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                SpawnPlayer(runner, player);
            }

            if (player == runner.LocalPlayer)
            {
                TryInitializeLocalPlayer(runner, player).Forget();
            }
        }

        private void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer)
                return;

            if (_spawnedPlayers.TryGetValue(player, out var playerObject))
            {
                runner.Despawn(playerObject);
                _spawnedPlayers.Remove(player);
            }
        }

        private void SpawnActivePlayers(NetworkRunner runner)
        {
            int index = 0;
            foreach (PlayerRef player in runner.ActivePlayers)
            {
                if (_spawnedPlayers.ContainsKey(player))
                    continue;

                SpawnPlayer(runner, player, index);
                index++;
            }
        }

        private void SpawnPlayer(NetworkRunner runner, PlayerRef player, int? indexOverride = null)
        {
            int index = indexOverride ?? _spawnedPlayers.Count;
            Vector3 spawnPosition = GetSpawnPosition(index);

            NetworkObject playerObject = runner.Spawn(
                _networkPlayerPrefab,
                spawnPosition,
                Quaternion.identity,
                player,
                OnBeforePlayerSpawned);

            _spawnedPlayers[player] = playerObject;
            runner.SetPlayerObject(player, playerObject);
        }

        private void OnBeforePlayerSpawned(NetworkRunner runner, NetworkObject obj)
        {
            _container.InjectGameObject(obj.gameObject);
            InitializeServerPlayer(obj.gameObject);
        }

        private async UniTaskVoid TryInitializeLocalPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (_localInitialized.Contains(player))
                return;

            NetworkObject playerObject = runner.GetPlayerObject(player);
            while (runner != null && runner.IsRunning)
            {
                if (playerObject != null && playerObject.GetComponent<PlayerController>() != null)
                    break;

                await UniTask.Yield();
                playerObject = runner.GetPlayerObject(player);
            }

            if (playerObject == null || playerObject.GetComponent<PlayerController>() == null)
                return;

            _localInitialized.Add(player);
            InitializeLocalPlayer(playerObject.gameObject);
        }

        private Vector3 GetSpawnPosition(int index)
        {
            if (_playerSpawnPoints != null && _playerSpawnPoints.Length > 0)
            {
                int clampedIndex = Mathf.Clamp(index, 0, _playerSpawnPoints.Length - 1);
                Transform spawnPoint = _playerSpawnPoints[clampedIndex];
                if (spawnPoint != null)
                    return spawnPoint.position;
            }

            return _playerSpawnPosition + new Vector2(SPACING * index, 0f);
        }

        private void InitializeServerPlayer(GameObject playerGo)
        {
            BulletGun bulletGun = playerGo.GetComponentInChildren<BulletGun>();
            LaserGun laserGun = playerGo.GetComponentInChildren<LaserGun>();
            IWeapon[] playerWeapons = { bulletGun, laserGun };

            PlayerController playerController = playerGo.GetComponent<PlayerController>();
            PlayerMovement playerMovement = playerGo.GetComponent<PlayerMovement>();

            playerController.GetComponent<CollisionHandler>().Initialize(_collisionService);
            playerMovement.Initialize(_playerConfigProvider);

            playerController.Initialize(playerMovement, new PlayerWeaponsHandler(playerWeapons));
            _enemyMovementConfigurator.Initialize(playerGo.transform);
            _boundsManager.RegisterObject(playerGo.transform);
            _pauseSystem.Register(playerController);
            _weaponsInitializer.Initialize(
                playerGo, _collisionService, playerWeapons, laserGun.GetComponentInChildren<ILineRenderer>());
            
            _playerControllerRegistry.Register(playerController);
        }

        private void InitializeLocalPlayer(GameObject playerGo)
        {
            PlayerController playerController = playerGo.GetComponent<PlayerController>();
            LaserGun laserGun = playerGo.GetComponentInChildren<LaserGun>();

            if (playerController == null || laserGun == null)
                return;

            _gameplaySessionManager.Initialize(playerController);
            _playerParamsService.Initialize(
                playerGo.transform, playerGo.GetComponent<Rigidbody2D>(), laserGun);
            
            _playerControllerRegistry.Register(playerController);
        }
    }
}
