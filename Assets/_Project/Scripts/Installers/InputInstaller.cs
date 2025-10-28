using _Project.Scripts.Input;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerInputReader>().AsSingle();
        }
    }
}