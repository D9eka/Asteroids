using _Project.Scripts.GameState;
using _Project.Scripts.Player;
using _Project.Scripts.Restarter;
using _Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
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
        }
    }
}