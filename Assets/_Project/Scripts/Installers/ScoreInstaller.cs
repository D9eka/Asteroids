using _Project.Scripts.Score;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class ScoreInstaller : MonoInstaller
    {
        [SerializeField] private ScoreConfig _scoreConfig;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle().WithArguments(_scoreConfig);
        }
    }
}