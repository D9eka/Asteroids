using UnityEngine;

namespace _Project.Scripts.Spawning.Enemies.Config
{
    [CreateAssetMenu(menuName = "Configs/AsteroidTypeConfig", fileName = "AsteroidTypeConfig")]
    public class AsteroidTypeConfig : EnemyTypeConfig
    {
        [Header("Asteroid Fragment")]
        [field: SerializeField] public GameObject FragmentPrefab { get; private set; }
        [field: SerializeField] public int MinFragments { get; private set; }
        [field: SerializeField] public int MaxFragments { get; private set; }
        [field: SerializeField] public float FragmentSpeedMultiplier { get; private set; }
        [field: SerializeField] public int FragmentPoolSize { get; private set; }
    }
}