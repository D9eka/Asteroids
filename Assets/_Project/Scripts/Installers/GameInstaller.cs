using _Project.Scripts.Collision;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICollisionService>().To<CollisionService>().AsSingle();
        }
    }
}