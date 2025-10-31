namespace Asteroids.Scripts.GameState
{
    public interface IGameStateController
    {
        void HandlePlayerDeath();
        void HandleRestartRequest();
    }
}