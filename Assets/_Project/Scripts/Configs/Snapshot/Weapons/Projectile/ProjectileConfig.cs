using System;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile
{
    [Serializable]
    public class ProjectileConfig
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }

        public ProjectileConfig(float speed, float lifeTime)
        {
            Speed = speed;
            LifeTime = lifeTime;
        }
    }
}