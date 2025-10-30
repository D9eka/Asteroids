using _Project.Scripts.Score;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class ScoreInstaller : MonoInstaller
    {
        [SerializeField] private ScoreConfig _scoreConfig;
        [SerializeField] private ScoreView _scoreView;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle().WithArguments(_scoreConfig);
            
            Container.Bind<ScoreView>().FromInstance(_scoreView).AsSingle();
        }
    }
}