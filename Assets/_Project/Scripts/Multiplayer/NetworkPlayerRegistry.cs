using System;
using System.Collections.Generic;
using Fusion;
using Zenject;

namespace _Project.Scripts.Multiplayer
{
    public class NetworkPlayerRegistry : IInitializable, IDisposable
    {
        private readonly NetworkEventsRouter _networkEventsRouter;
        private readonly Dictionary<PlayerRef, NetworkPlayer> _players = new();

        public IReadOnlyDictionary<PlayerRef, NetworkPlayer> Players => _players;

        public NetworkPlayerRegistry(NetworkEventsRouter networkEventsRouter)
        {
            _networkEventsRouter = networkEventsRouter;
        }

        public void Initialize()
        {
            _networkEventsRouter.ObjectEnterAOIEvent += OnObjectEnterAOI;
            _networkEventsRouter.ObjectExitAOIEvent += OnObjectExitAOI;
            _networkEventsRouter.ShutdownEvent += OnShutdown;
        }

        public void Dispose()
        {
            _networkEventsRouter.ObjectEnterAOIEvent -= OnObjectEnterAOI;
            _networkEventsRouter.ObjectExitAOIEvent -= OnObjectExitAOI;
            _networkEventsRouter.ShutdownEvent -= OnShutdown;
            _players.Clear();
        }

        public bool TryGet(PlayerRef playerRef, out NetworkPlayer player)
        {
            return _players.TryGetValue(playerRef, out player);
        }

        private void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            if (runner == null || player != runner.LocalPlayer)
                return;

            if (obj == null || !obj.TryGetComponent(out NetworkPlayer networkPlayer))
                return;

            _players[networkPlayer.Object.InputAuthority] = networkPlayer;
        }

        private void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            if (runner == null || player != runner.LocalPlayer)
                return;

            if (obj == null || !obj.TryGetComponent(out NetworkPlayer networkPlayer))
                return;

            _players.Remove(networkPlayer.Object.InputAuthority);
        }

        private void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            _players.Clear();
        }
    }
}
