using System;
using _Project.Scripts.Spawning.Enemies.Config;

namespace _Project.Scripts.Spawning.Enemies.Providers
{
    public interface IEnemyProviderFactory
    {
        public Type ConfigType { get; }
    
        public IEnemyProvider Create(EnemyTypeConfig config);
    }
}