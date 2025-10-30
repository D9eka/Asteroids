namespace _Project.Scripts.Pause
{
    public interface IPauseSystem
    {
        bool IsPaused { get; }
        void Pause();
        void Resume();
        void Register(IPausable pausable);
        void Unregister(IPausable pausable);
    }
}