namespace _Project.Scripts.Spawning.Enemies.Providers
{
    public interface IEnemyProvider
    {
        public float Probability { get; }
        public float SpawnInterval { get; } 
    }
}