using _Project.Scripts.Collision;
using _Project.Scripts.Damage;
using _Project.Scripts.Weapons.Types.BulletGun;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class Ufo : MonoBehaviour, IEnemy
    {
        public event Action<GameObject, IEnemy> OnKilled;
        
        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }
        [field: SerializeField] public BulletGun BulletGun { get; private set; }
        
        public Transform Transform => transform;

        private void Update()
        {
            if (BulletGun.CanShoot)
            {
                BulletGun.Shoot();
            }
        }

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);

        public void TakeDamage(DamageInfo damageInfo)
        {
            OnKilled?.Invoke(damageInfo.Instigator, this);
        }

        public DamageInfo GetDamageInfo()
        {
            return new DamageInfo(DamageType.Collide, gameObject);
        }
    }
}