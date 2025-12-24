using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Weapons.Projectile
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Weapons/ProjectileConfig")]
    public class ProjectileConfig : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }
    }
}