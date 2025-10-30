using _Project.Scripts.Spawning.Enemies.Config;
using UnityEngine;

namespace _Project.Scripts.Spawning.Enemies.Fragments
{
    public interface IAsteroidFragmentFactory
    {
        void SpawnFragments(Vector2 center, float asteroidSpeed, AsteroidFragmentTypeSpawnConfig spawnConfig);
    }
}