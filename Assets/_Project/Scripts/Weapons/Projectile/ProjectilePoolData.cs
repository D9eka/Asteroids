using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Configs.Snapshot.Weapons.Projectile;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs.Colliders.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectilePoolData
    {
        public int Id { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public ProjectileConfig Config { get; private set; }
        public DamageInfo DamageInfo { get; private set; }
        public ICollisionService CollisionService { get; private set; }

        public ProjectilePoolData(int id, Vector3 position, Quaternion rotation, 
            ProjectileConfig config, DamageInfo damageInfo, ICollisionService collisionService)
        {
            Id = id;
            Position = position;
            Rotation = rotation;
            Config = config;
            DamageInfo = damageInfo;
            CollisionService = collisionService;
        }
    }
}