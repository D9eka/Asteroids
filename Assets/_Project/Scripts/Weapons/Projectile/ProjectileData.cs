using UnityEngine;

namespace _Project.Scripts.Weapons.Projectile
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Weapons/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}