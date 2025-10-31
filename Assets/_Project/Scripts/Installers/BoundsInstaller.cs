using Asteroids.Scripts.Camera;
using Asteroids.Scripts.WarpSystem;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Installers
{
    public class BoundsInstaller : MonoInstaller
    {
        [SerializeField] private float _boundsMargin;
        
        public override void InstallBindings()
        {
            Container.Bind<float>().WithId("BoundsMargin").FromInstance(_boundsMargin).AsCached();
            Container.Bind<ICameraBoundsUpdater>().To<CameraBoundsUpdater>().AsSingle().NonLazy();
            Container.Bind<IBoundsWarp>().To<CameraBoundsWarp>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BoundsManager>().AsSingle().NonLazy();
        }
    }
}