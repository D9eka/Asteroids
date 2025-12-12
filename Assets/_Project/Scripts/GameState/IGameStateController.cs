using System;
using Asteroids.Scripts.Player;
using UniRx;

namespace Asteroids.Scripts.GameState
{
    public interface IGameStateController
    {
        public IObservable<Unit> PlayerDeath { get; }
        public void Initialize(IPlayerController playerController);
        public void HandleRestartRequest();
        public void HandleExitRequest();
    }
}