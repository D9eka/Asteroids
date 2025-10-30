namespace _Project.Scripts.GameState
{
    public interface IGameStateController
    {
        void HandlePlayerDeath();
        void HandleRestartRequest();
    }
}