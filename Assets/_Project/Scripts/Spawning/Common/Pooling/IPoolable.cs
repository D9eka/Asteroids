namespace Asteroids.Scripts.Spawning.Common.Pooling
{
    public interface IPoolable
    {
        bool Enabled { get; }
        void OnSpawned();
        void OnDespawned();
    }
}