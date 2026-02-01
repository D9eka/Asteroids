using Fusion;
using UnityEngine;
using _Project.Scripts.Multiplayer.Pooling;
using Zenject;

namespace _Project.Scripts.Multiplayer
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private NetworkEventsRouter _eventsRouterPrefab;
        [SerializeField] private NetworkObject _networkPlayerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<NetworkEventsRouter>()
                .FromComponentInNewPrefab(_eventsRouterPrefab)
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerControllerRegistry>().AsSingle().NonLazy();
            Container.Bind<NetworkObjectPoolRegistry>().AsSingle();
            Container.Bind<PlayerSpawner>().AsSingle().WithArguments(_networkPlayerPrefab);
        }
    }
}
