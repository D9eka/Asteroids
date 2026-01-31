using System;
using System.Threading;
using _Project.Scripts.Multiplayer;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.Multiplayer.PlayerListUi
{
    public class PlayerListPresenter : IInitializable, IDisposable
    {
        private readonly PlayerList _playerList;
        private readonly NetworkEventsRouter _networkEventsRouter;
        private readonly NetworkPlayerRegistry _playerRegistry;
        private readonly CancellationTokenSource _cts = new();

        private bool _refreshInProgress;
        private bool _refreshQueued;
        private bool _subscribed;

        public PlayerListPresenter(PlayerList playerList, NetworkEventsRouter networkEventsRouter, NetworkPlayerRegistry playerRegistry)
        {
            _playerList = playerList;
            _networkEventsRouter = networkEventsRouter;
            _playerRegistry = playerRegistry;
        }

        public void Initialize()
        {
            Subscribe();
            RequestRefresh();
        }

        public void Dispose()
        {
            Unsubscribe();
            _cts.Cancel();
            _cts.Dispose();
        }

        private void Subscribe()
        {
            if (_subscribed)
                return;

            _subscribed = true;
            _networkEventsRouter.SceneLoadDoneEvent += OnSceneLoadDone;
            _networkEventsRouter.PlayerJoinedEvent += OnPlayerChanged;
            _networkEventsRouter.PlayerLeftEvent += OnPlayerChanged;
        }

        private void Unsubscribe()
        {
            if (!_subscribed || _networkEventsRouter == null)
                return;

            _subscribed = false;
            _networkEventsRouter.SceneLoadDoneEvent -= OnSceneLoadDone;
            _networkEventsRouter.PlayerJoinedEvent -= OnPlayerChanged;
            _networkEventsRouter.PlayerLeftEvent -= OnPlayerChanged;
        }

        private void OnSceneLoadDone(NetworkRunner runner)
        {
            RequestRefresh();
        }

        private void OnPlayerChanged(NetworkRunner runner, PlayerRef player)
        {
            RequestRefresh();
        }

        private void RequestRefresh()
        {
            if (_cts.IsCancellationRequested)
                return;

            if (_refreshInProgress)
            {
                _refreshQueued = true;
                return;
            }

            RefreshInternal(_cts.Token).Forget();
        }

        private async UniTask RefreshInternal(CancellationToken token)
        {
            _refreshInProgress = true;
            do
            {
                _refreshQueued = false;
                await RefreshOnce(token);
            }
            while (_refreshQueued && !token.IsCancellationRequested);
            _refreshInProgress = false;
        }

        private async UniTask RefreshOnce(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            NetworkRunner runner = _networkEventsRouter.GetAttachedRunner();
            if (runner == null)
                return;

            await WaitNicknamesLoading(runner, token);

            if (token.IsCancellationRequested)
                return;

            _playerList.Clear();
            foreach (PlayerRef playerRef in runner.CommittedPlayers)
            {
                if (!TryGetNetworkPlayer(playerRef, out NetworkPlayer player))
                    continue;

                string nickname = player.GetNickname();
                bool isHost = player.IsHost;
                bool isLocal = playerRef == runner.LocalPlayer;
                _playerList.Add(nickname, isHost, isLocal);
            }
        }

        private async UniTask WaitNicknamesLoading(NetworkRunner runner, CancellationToken token)
        {
            bool allLoaded = false;
            int attempts = 0;
            const int maxAttempts = 100;

            while (!allLoaded && attempts < maxAttempts && !token.IsCancellationRequested)
            {
                allLoaded = true;

                foreach (PlayerRef playerRef in runner.ActivePlayers)
                {
                    if (!TryGetNetworkPlayer(playerRef, out NetworkPlayer player) || !IsPlayerLoaded(player))
                    {
                        allLoaded = false;
                        break;
                    }
                }

                if (!allLoaded)
                {
                    await UniTask.Delay(50, cancellationToken: token);
                    attempts++;
                }
            }
        }

        private bool IsPlayerLoaded(NetworkPlayer player)
        {
            return !string.IsNullOrEmpty(player.GetNickname());
        }

        private bool TryGetNetworkPlayer(PlayerRef playerRef, out NetworkPlayer player)
        {
            player = null;
            if (_playerRegistry == null)
                return false;

            return _playerRegistry.TryGet(playerRef, out player);
        }
    }
}
