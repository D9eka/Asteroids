using System;
using System.Collections.Generic;
using Asteroids.Scripts.Player;
using Fusion;
using Zenject;

namespace _Project.Scripts.Multiplayer
{
    public class PlayerControllerRegistry : IInitializable, IDisposable
    {
        public event Action<IPlayerController> OnPlayerAdded;
        
        private readonly NetworkEventsRouter _networkEventsRouter;
        private readonly Dictionary<PlayerRef, IPlayerController> _players = new();

        public IReadOnlyDictionary<PlayerRef, IPlayerController> Players => _players;

        public PlayerControllerRegistry(NetworkEventsRouter networkEventsRouter)
        {
            _networkEventsRouter = networkEventsRouter;
        }

        public void Initialize()
        {
            _networkEventsRouter.ShutdownEvent += OnShutdown;
            _networkEventsRouter.SceneLoadDoneEvent += OnSceneLoadDone;
            _networkEventsRouter.PlayerJoinedEvent += OnPlayerChanged;
            _networkEventsRouter.PlayerLeftEvent += OnPlayerChanged;
        }

        public void Dispose()
        {
            _networkEventsRouter.ShutdownEvent -= OnShutdown;
            _networkEventsRouter.SceneLoadDoneEvent -= OnSceneLoadDone;
            _networkEventsRouter.PlayerJoinedEvent -= OnPlayerChanged;
            _networkEventsRouter.PlayerLeftEvent -= OnPlayerChanged;
            _players.Clear();
        }

        public bool TryGet(PlayerRef playerRef, out IPlayerController player)
        {
            return _players.TryGetValue(playerRef, out player);
        }

        public void Register(PlayerController player)
        {
            if (player == null || player.Object == null)
                return;
            
            _players[player.Object.InputAuthority] = player;
            OnPlayerAdded?.Invoke(player);
        }

        public void Unregister(PlayerController player)
        {
            if (player == null || player.Object == null)
                return;

            _players.Remove(player.Object.InputAuthority);
        }

        private void OnSceneLoadDone(NetworkRunner runner)
        {
            PruneMissingPlayers(runner);
        }

        private void OnPlayerChanged(NetworkRunner runner, PlayerRef player)
        {
            if (runner == null)
                return;

            if (!runner.IsRunning)
            {
                _players.Clear();
                return;
            }

            if (!IsActivePlayer(runner, player))
                _players.Remove(player);
        }

        private void PruneMissingPlayers(NetworkRunner runner)
        {
            if (runner == null)
                return;

            List<PlayerRef> toRemove = null;
            foreach (var entry in _players)
            {
                if (!IsActivePlayer(runner, entry.Key))
                {
                    toRemove ??= new List<PlayerRef>();
                    toRemove.Add(entry.Key);
                }
            }

            if (toRemove == null)
                return;

            foreach (PlayerRef playerRef in toRemove)
                _players.Remove(playerRef);
        }

        private bool IsActivePlayer(NetworkRunner runner, PlayerRef player)
        {
            foreach (PlayerRef activePlayer in runner.ActivePlayers)
            {
                if (activePlayer == player)
                    return true;
            }

            return false;
        }

        private void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            _players.Clear();
        }
    }
}
