using System;
using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Spawning.Enemies.Core;
using Fusion;
using UnityEngine;

namespace Asteroids.Scripts.Enemies
{
    public class AsteroidFragment : NetworkBehaviour, IEnemy
    {
        public event Action<GameObject, IEnemy> OnKilled;
        
        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }
        
        private IEnemyFactory _fragmentsFactory;
        
        [Networked] private Vector2 NetPosition { get; set; }
        [Networked] private float NetRotation { get; set; }
        
        public Transform Transform => transform;
        public bool Enabled => gameObject.activeSelf;
        public EnemyType Type { get; private set; }
        
        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority)
                return;

            NetPosition = transform.position;
            NetRotation = transform.rotation.eulerAngles.z;
        }

        public override void Render()
        {
            if (Object.HasStateAuthority)
                return;

            transform.SetPositionAndRotation(NetPosition, Quaternion.Euler(0f, 0f, NetRotation));
        }

        public void SetType(EnemyType type)
        {
            Type = type;
        }

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (!Object.HasStateAuthority)
                return;

            OnKilled?.Invoke(damageInfo.Instigator, this);
        }

        public DamageInfo GetDamageInfo()
        {
            return new DamageInfo(DamageType.Collide, gameObject);
        }

        public void Pause()
        {
            Movement.Pause();
        }

        public void Resume()
        {
            Movement.Resume();
        }
    }
}