using _Project.Scripts.Advertisement;
using Asteroids.Scripts.GameState;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.UI.Screens.EndGameScreen
{
    public class ReviveFlowController : IInitializable
    {
        private readonly IGameStateController _gameStateController;
        private readonly IUIController _uiController;
        private readonly IAdvertisementSystem _advertisementSystem;
        
        private ReviveScreenView _reviveScreenView;

        public ReviveFlowController(IGameStateController gameStateController, IUIController uiController, 
            IAdvertisementSystem advertisementSystem)
        {
            _gameStateController = gameStateController;
            _uiController = uiController;
            _advertisementSystem = advertisementSystem;
        }

        public void Initialize()
        {
            _gameStateController.PlayerDeath.Subscribe(_ => ActivateEndGameScreen());
            _advertisementSystem.RevivalRewardGranted.Subscribe(_ => DisableRevivalScreen());
        }

        public void Initialize(ReviveScreenView reviveScreenView)
        {
            _reviveScreenView = reviveScreenView;
        }

        private void ActivateEndGameScreen()
        {
            if (_advertisementSystem.CanRevive)
            {
                _uiController.OpenScreen(_reviveScreenView);
            }
        }

        private void DisableRevivalScreen()
        {
            _uiController.CloseScreen(_reviveScreenView);
        }
    }
}