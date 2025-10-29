using System.Collections.Generic;
using _Project.Scripts.Spawning.Enemies.Providers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Spawning.Enemies.Core
{
    public class EnemySpawner : ITickable
    {
        private readonly IEnemyFactory _factory;
        private readonly List<IEnemyProvider> _providers;
        private readonly Dictionary<IEnemyProvider, float> _timers = new();

        public EnemySpawner(IEnemyFactory factory, List<IEnemyProvider> providers)
        {
            _factory = factory;
            _providers = providers;

            foreach (var provider in _providers)
                _timers[provider] = 0f;
        }

        public void Tick()
        {
            foreach (var provider in _providers)
            {
                _timers[provider] -= Time.deltaTime;

                if (_timers[provider] <= 0f)
                {
                    if (Random.value <= provider.Probability)
                        _factory.Spawn();

                    _timers[provider] = provider.SpawnInterval;
                }
            }
        }
    }
}