using Asteroids.Scripts.Configs.Authoring.Enemies.SpawnConfig;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Enemies
{
    [CreateAssetMenu(menuName = "Configs/Enemy/AsteroidFragmentTypeSpawnConfig", fileName = "AsteroidFragmentTypeSpawnConfig")]
    public class AsteroidTypeConfigSo : EnemyTypeConfigSo
    {
        [Header("Asteroid Fragment")]
        [field: SerializeField] public AsteroidFragmentTypeSpawnConfigSo AsteroidFragmentSpawnConfigSo { get; private set; }
    }
}