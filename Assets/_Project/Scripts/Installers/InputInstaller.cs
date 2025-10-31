using Asteroids.Scripts.Input;
using Zenject;

namespace Asteroids.Scripts.Installers
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerInputReader>().AsSingle();
        }
    }
}