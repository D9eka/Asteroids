using Asteroids.Scripts.Spawning.Enemies.Config;
using UnityEngine;

namespace Asteroids.Scripts.Enemies.Config
{
    [CreateAssetMenu(menuName = "Configs/Enemy/AsteroidFragmentTypeSpawnConfig", fileName = "AsteroidFragmentTypeSpawnConfig")]
    public class AsteroidTypeConfig : EnemyTypeConfig
    {
        [Header("Asteroid Fragment")]
        [field: SerializeField] public AsteroidFragmentTypeSpawnConfig AsteroidFragmentSpawnConfig { get; private set; }
    }
}