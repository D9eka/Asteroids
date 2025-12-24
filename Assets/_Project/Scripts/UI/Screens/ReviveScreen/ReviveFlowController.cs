using Asteroids.Scripts.Advertisement;
using Asteroids.Scripts.GameState;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.UI.Screens.ReviveScreen
{
    public class ReviveFlowController : IInitializable
    {
        private readonly IGameStateController _gameStateController;
        private readonly IUIController _uiController;
        private readonly IAdvertisementService _advertisementService;
        
        private ReviveScreenView _reviveScreenView;

        public ReviveFlowController(IGameStateController gameStateController, IUIController uiController, 
            IAdvertisementService advertisementService)
        {
            _gameStateController = gameStateController;
            _uiController = uiController;
            _advertisementService = advertisementService;
        }

        public void Initialize()
        {
            _gameStateController.PlayerDeath.Subscribe(_ => ActivateEndGameScreen());
            _advertisementService.RevivalRewardGranted.Subscribe(_ => DisableRevivalScreen());
        }

        public void Initialize(ReviveScreenView reviveScreenView)
        {
            _reviveScreenView = reviveScreenView;
        }

        private void ActivateEndGameScreen()
        {
            if (_advertisementService.CanRevive)
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