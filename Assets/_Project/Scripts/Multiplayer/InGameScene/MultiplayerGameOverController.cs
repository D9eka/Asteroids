using System;
using System.Collections.Generic;
using System.Threading;
using Asteroids.Scripts.Effects.Explosion;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Multiplayer.InGameScene
{
    public class MultiplayerGameOverController : IInitializable, IDisposable
    {
        private const float RETURN_DELAY_SECONDS = 3f;

        private readonly NetworkEventsRouter _networkEventsRouter;
        private readonly ExplosionEffectFactory _explosionEffectFactory;
        private readonly ExplosionSoundData _explosionSoundData;
        private readonly PlayerControllerRegistry _playerControllerRegistry;
        private readonly IPauseSystem _pauseSystem;
        private readonly SceneRef _lobbySceneRef;
        
        private readonly HashSet<IPlayerController> _subscribedPlayers = new();
        private readonly Dictionary<IPlayerController, PlayerRef> _playerRefs = new();
        private readonly HashSet<PlayerRef> _deadPlayers = new();
        private readonly CancellationTokenSource _cts = new();

        private bool _gameOverTriggered;

        public MultiplayerGameOverController(NetworkEventsRouter networkEventsRouter,
            ExplosionEffectFactory explosionEffectFactory,
            ExplosionSoundData explosionSoundData,
            PlayerControllerRegistry playerControllerRegistry,
            IPauseSystem pauseSystem,
            SceneRef lobbySceneRef)
        {
            _networkEventsRouter = networkEventsRouter;
            _explosionEffectFactory = explosionEffectFactory;
            _explosionSoundData = explosionSoundData;
            _playerControllerRegistry = playerControllerRegistry;
            _pauseSystem = pauseSystem;
            _lobbySceneRef = lobbySceneRef;
        }

        public void Initialize()
        {
            _playerControllerRegistry.OnPlayerAdded += PlayerControllerRegistryOnPlayerControllerAdded;
            foreach (KeyValuePair<PlayerRef, IPlayerController> networkPlayer in _playerControllerRegistry.Players)
                RegisterPlayer(networkPlayer.Key, networkPlayer.Value);
        }
        
        private void PlayerControllerRegistryOnPlayerControllerAdded(IPlayerController playerController)
        {
            if (!TryResolvePlayerRef(playerController, out PlayerRef playerRef))
                return;

            RegisterPlayer(playerRef, playerController);
        }

        private void OnPlayerKilled(IPlayerController player)
        {
            if (!TryResolvePlayerRef(player, out PlayerRef playerRef))
                return;

            if (!_deadPlayers.Add(playerRef))
                return;

            NetworkRunner runner = _networkEventsRouter.GetAttachedRunner();
            if (runner == null || !runner.IsRunning || !runner.IsServer)
                return;
            
            int randomSoundIndex = Random.Range(0, _explosionSoundData.ExplosionSounds.Length);
            _explosionEffectFactory.Create(player.Transform.position, _explosionSoundData.ExplosionSounds[randomSoundIndex]);

            int totalPlayers = _playerControllerRegistry.Players.Count;
            if (!_gameOverTriggered && totalPlayers > 0 && _deadPlayers.Count >= totalPlayers)
            {
                _gameOverTriggered = true;
                ReturnToLobbyAfterDelay().Forget();
            }
        }

        public void Dispose()
        {
            _playerControllerRegistry.OnPlayerAdded -= PlayerControllerRegistryOnPlayerControllerAdded;
            foreach (IPlayerController player in _subscribedPlayers)
                player.OnKilled -= OnPlayerKilled;
            _subscribedPlayers.Clear();
            _playerRefs.Clear();
            _deadPlayers.Clear();
            
            _cts.Cancel();
            _cts.Dispose();
        }

        private async UniTaskVoid ReturnToLobbyAfterDelay()
        {
            _pauseSystem.Pause();
            await UniTask.Delay(TimeSpan.FromSeconds(RETURN_DELAY_SECONDS), cancellationToken: _cts.Token);

            NetworkRunner runner = _networkEventsRouter.GetAttachedRunner();
            if (runner == null || !runner.IsRunning || !runner.IsServer)
                return;

            DespawnAllPlayers(runner);
            await runner.Shutdown();
            SceneManager.LoadScene(_lobbySceneRef.AsIndex);
        }

        private void DespawnAllPlayers(NetworkRunner runner)
        {
            foreach (PlayerRef playerRef in runner.ActivePlayers)
            {
                NetworkObject playerObject = runner.GetPlayerObject(playerRef);
                if (playerObject != null)
                    runner.Despawn(playerObject);
            }
        }

        private void RegisterPlayer(PlayerRef playerRef, IPlayerController playerController)
        {
            if (playerController == null)
                return;

            if (!_subscribedPlayers.Add(playerController))
                return;

            _playerRefs[playerController] = playerRef;
            playerController.OnKilled += OnPlayerKilled;
        }

        private bool TryResolvePlayerRef(IPlayerController playerController, out PlayerRef playerRef)
        {
            if (playerController != null && _playerRefs.TryGetValue(playerController, out playerRef))
                return true;

            foreach (KeyValuePair<PlayerRef, IPlayerController> entry in _playerControllerRegistry.Players)
            {
                if (entry.Value == playerController)
                {
                    playerRef = entry.Key;
                    _playerRefs[playerController] = playerRef;
                    return true;
                }
            }

            playerRef = default;
            return false;
        }
    }
}
