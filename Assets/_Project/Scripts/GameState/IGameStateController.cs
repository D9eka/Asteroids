using System;
using UniRx;

namespace Asteroids.Scripts.GameState
{
    public interface IGameStateController
    {
        public IObservable<Unit> PlayerDeath { get; }
        void HandleRestartRequest();
    }
}