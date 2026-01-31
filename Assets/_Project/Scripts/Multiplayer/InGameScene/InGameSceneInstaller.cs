using _Project.Scripts.Multiplayer.PlayerListUi;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Multiplayer.InGameScene
{
    public class InGameSceneInstaller : MonoInstaller
    {
        [SerializeField] private PlayerList _playerListInstance;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerList>().FromInstance(_playerListInstance).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerListPresenter>().AsSingle().NonLazy();
        }
    }
}
