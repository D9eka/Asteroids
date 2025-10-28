using _Project.Scripts.Collision;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private UnityEngine.Camera _camera;
        
        public override void InstallBindings()
        {
            Container.Bind<UnityEngine.Camera>().FromInstance(_camera).AsSingle();
            
            Container.Bind<ICollisionService>()
                .WithId("PlayerCollisionService")
                .To<PlayerCollisionService>()
                .AsCached();
        }
    }
}