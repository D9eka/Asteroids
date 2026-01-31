using _Project.Scripts.Multiplayer.PlayerListUi;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Multiplayer.Lobby
{
    public class LobbySceneInstaller : MonoInstaller
    {
        [SerializeField] private LobbyUI _lobbyUIInstance;
        [SerializeField] private PlayerList _playerListInstance;
        [Space]
        [SerializeField] private NetworkRunner _networkRunnerPrefab;
        [SerializeField] private SceneRef _gameSceneRef;
        
        public override void InstallBindings()
        {
            Container.Bind<LobbyUI>().FromInstance(_lobbyUIInstance).AsSingle();
            Container.Bind<PlayerList>().FromInstance(_playerListInstance).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerListPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LobbyController>().AsSingle()
                .WithArguments(_networkRunnerPrefab, _gameSceneRef).NonLazy();
        }
    }
}
