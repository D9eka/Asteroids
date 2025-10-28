using _Project.Scripts.Collision;
using _Project.Scripts.Damage;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class Asteroid : MonoBehaviour, IEnemy
    {
        [field: SerializeField] public CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (damageInfo.Type == DamageType.Bullet)
            {
                //TODO: spawn asteroids fragments
                Debug.Log("BOOM");
            }
            OnDespawned();
        }

        public DamageInfo GetDamageInfo()
        {
            return new DamageInfo(DamageType.Collide, gameObject);
        }
    }
}