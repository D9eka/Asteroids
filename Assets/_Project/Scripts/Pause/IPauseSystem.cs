namespace Asteroids.Scripts.Pause
{
    public interface IPauseSystem
    {
        void Pause();
        void Resume();
        void Register(IPausable pausable);
        void Register(ITickableSystem pausable);
    }
}