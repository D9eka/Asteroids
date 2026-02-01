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
            _networkEventsRouter.ObjectEnterAOIEvent += OnObjectEnterAoi;
            _networkEventsRouter.ObjectExitAOIEvent += OnObjectExitAoi;
        }

        public void Dispose()
        {
            _networkEventsRouter.ShutdownEvent -= OnShutdown;
            _networkEventsRouter.SceneLoadDoneEvent -= OnSceneLoadDone;
            _networkEventsRouter.PlayerJoinedEvent -= OnPlayerChanged;
            _networkEventsRouter.PlayerLeftEvent -= OnPlayerChanged;
            _networkEventsRouter.ObjectEnterAOIEvent -= OnObjectEnterAoi;
            _networkEventsRouter.ObjectExitAOIEvent -= OnObjectExitAoi;
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

            PlayerRef playerRef = player.Object.InputAuthority;
            if (_players.TryGetValue(playerRef, out IPlayerController existing) && ReferenceEquals(existing, player))
                return;

            _players[playerRef] = player;
            OnPlayerAdded?.Invoke(player);
        }

        public void Register(PlayerRef playerRef, PlayerController player)
        {
            if (player == null)
                return;

            if (_players.TryGetValue(playerRef, out IPlayerController existing) && ReferenceEquals(existing, player))
                return;

            _players[playerRef] = player;
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
            SyncPlayersFromRunner(runner);
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

            SyncPlayersFromRunner(runner);
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

        private void OnObjectEnterAoi(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            if (obj == null)
                return;

            if (obj.TryGetComponent(out PlayerController controller))
                Register(obj.InputAuthority, controller);
        }

        private void OnObjectExitAoi(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            if (obj == null)
                return;

            if (obj.TryGetComponent(out PlayerController controller))
                Unregister(controller);
        }

        private void SyncPlayersFromRunner(NetworkRunner runner)
        {
            if (runner == null)
                return;

            foreach (PlayerRef playerRef in runner.ActivePlayers)
            {
                if (!runner.TryGetPlayerObject(playerRef, out NetworkObject playerObject) || playerObject == null)
                    continue;

                if (playerObject.TryGetComponent(out PlayerController controller))
                    Register(playerRef, controller);
            }
        }
    }
}
