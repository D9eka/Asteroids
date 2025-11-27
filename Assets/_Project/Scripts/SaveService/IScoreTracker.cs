using UniRx;

namespace Asteroids.Scripts.SaveService
{
    public interface IScoreTracker
    {
        IReadOnlyReactiveProperty<int> PreviousScore { get; }
        IReadOnlyReactiveProperty<int> HighestScore { get; }
        
        public void SaveCurrentScore();
    }
}