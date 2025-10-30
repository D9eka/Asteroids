using Zenject;

namespace _Project.Scripts.Player
{
    public class PlayerDeathHandler : IInitializable
    {
        private readonly IPlayerController _playerController;

        public PlayerDeathHandler(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Initialize()
        {
            _playerController.OnKilled += PlayerControllerOnOnKilled;
        }

        private void PlayerControllerOnOnKilled()
        {
            throw new System.NotImplementedException();
        }
    }
}