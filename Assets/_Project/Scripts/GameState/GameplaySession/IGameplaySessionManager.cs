using Asteroids.Scripts.Player;
using Asteroids.Scripts.UI.Screens;

namespace Asteroids.Scripts.GameState.GameplaySession
{
    public interface IGameplaySessionManager
    {
        public void Initialize(IPlayerController playerController);
        public void Initialize(IView gameplayView);
        public void Start();
        public void Reset();
        public void Restart();
    }
}