using System;
using System.Threading;
using Asteroids.Scripts.Player;
using Cysharp.Threading.Tasks;
using Fusion;
using UniRx;
using Zenject;

namespace _Project.Scripts.Multiplayer.PlayerStats
{
    public class PlayersStatsViewModel : IFixedTickable
    {
        private readonly NetworkEventsRouter _networkEventsRouter;

        public ReactiveProperty<string> LocalPlayerNickname { get; private set; } = new();
        public ReactiveProperty<int> LocalPlayerScore { get; private set; } = new();

        public ReactiveProperty<string> NetworkPlayerNickname { get; private set; } = new();
        public ReactiveProperty<int> NetworkPlayerScore { get; private set; } = new();

        public PlayersStatsViewModel(NetworkEventsRouter networkEventsRouter)
        {
            _networkEventsRouter = networkEventsRouter;
        }

        public void FixedTick()
        {
            RefreshOnce();
        }

        private void RefreshOnce()
        {
            NetworkRunner runner = _networkEventsRouter.GetAttachedRunner();
            if (runner == null)
                return;

            PlayerController localController = GetPlayerController(runner, runner.LocalPlayer);
            PlayerController remoteController = null;
            foreach (PlayerRef playerRef in runner.ActivePlayers)
            {
                if (playerRef == runner.LocalPlayer)
                    continue;

                remoteController = GetPlayerController(runner, playerRef);
                break;
            }

            if (localController != null)
            {
                LocalPlayerNickname.Value = localController.Nickname;
                LocalPlayerScore.Value = localController.Score;
            }
            else
            {
                LocalPlayerNickname.Value = string.Empty;
                LocalPlayerScore.Value = 0;
            }

            if (remoteController != null)
            {
                NetworkPlayerNickname.Value = remoteController.Nickname;
                NetworkPlayerScore.Value = remoteController.Score;
            }
            else
            {
                NetworkPlayerNickname.Value = string.Empty;
                NetworkPlayerScore.Value = 0;
            }
        }

        private static PlayerController GetPlayerController(NetworkRunner runner, PlayerRef playerRef)
        {
            if (runner == null)
                return null;

            if (!runner.TryGetPlayerObject(playerRef, out NetworkObject playerObject))
                return null;

            if (playerObject == null)
                return null;

            playerObject.TryGetComponent(out PlayerController controller);
            return controller;
        }
    }
}
