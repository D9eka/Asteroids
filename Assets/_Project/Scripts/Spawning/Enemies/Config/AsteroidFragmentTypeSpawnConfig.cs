using UnityEngine;

namespace _Project.Scripts.Spawning.Enemies.Config
{
    [CreateAssetMenu(menuName = "Configs/Spawn/Enemy/AsteroidFragmentTypeSpawnConfig", fileName = "AsteroidFragmentTypeSpawnConfig")]
    public class AsteroidFragmentTypeSpawnConfig : EnemyTypeSpawnConfig
    {
        [Header("Asteroid Fragment")]
        [field: SerializeField] public int MinFragments { get; private set; }
        [field: SerializeField] public int MaxFragments { get; private set; }
        [field: SerializeField] public float FragmentPositionOffsetModefier { get; private set; }
        [field: SerializeField] public float FragmentSpeedMultiplier { get; private set; }
    }
}