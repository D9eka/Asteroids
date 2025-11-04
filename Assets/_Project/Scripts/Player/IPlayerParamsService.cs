using UniRx;

namespace Asteroids.Scripts.Player
{
    public interface IPlayerParamsService
    {
        public IReadOnlyReactiveProperty<string> Params { get; }
    }
}