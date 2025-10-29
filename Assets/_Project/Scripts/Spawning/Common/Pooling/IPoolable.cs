namespace _Project.Scripts.Spawning.Common.Pooling
{
    public interface IPoolable
    {
        void OnSpawned();
        void OnDespawned();
    }
}