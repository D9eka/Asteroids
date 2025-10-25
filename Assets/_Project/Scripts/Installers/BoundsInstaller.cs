using _Project.Scripts.Camera;
using _Project.Scripts.WarpSystem;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
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