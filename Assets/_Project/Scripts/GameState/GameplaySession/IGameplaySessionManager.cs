using System;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.UI.Screens;
using UniRx;

namespace Asteroids.Scripts.GameState.GameplaySession
{
    public interface IGameplaySessionManager
    {
        public IObservable<Unit> GameStarted { get; }
        
        public void Initialize(IPlayerController playerController);
        public void Initialize(IView gameplayView);
        public void Start();
        public void Reset();
        public void Restart();
    }
}