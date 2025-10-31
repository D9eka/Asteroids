using Asteroids.Scripts.GameState;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Restarter;
using Asteroids.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Installers
{
    public class GameStateInstaller : MonoInstaller
    {
        [SerializeField] private ShowPlayerParams _showPlayerParams;
        [SerializeField] private RestartGameButton _restartButton;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShowPlayerParams>().FromInstance(_showPlayerParams).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerParamsService>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<RestartGameButton>().FromInstance(_restartButton).AsSingle();
            Container.BindInterfacesAndSelfTo<GameRestarter>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateController>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<PauseSystem>().AsSingle();
        }
    }
}