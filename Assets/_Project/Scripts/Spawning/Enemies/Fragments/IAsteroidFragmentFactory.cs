using Asteroids.Scripts.Spawning.Enemies.Config;
using UnityEngine;

namespace Asteroids.Scripts.Spawning.Enemies.Fragments
{
    public interface IAsteroidFragmentFactory
    {
        void SpawnFragments(Vector2 center, Vector2 hitDirection, float asteroidSpeed, 
            AsteroidFragmentTypeSpawnConfig spawnConfig);
    }
}