using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public class BulletGun : MonoBehaviour, IWeapon
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _fireRate = 0.25f;

        private float _cooldown;

        public bool CanShoot => _cooldown <= 0f;

        private void Update()
        {
            Recharge();
        }

        public void Shoot()
        {
            if (!CanShoot) return;
            Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
            _cooldown = _fireRate;
        }

        public void Recharge()
        {
            if (_cooldown > 0)
                _cooldown -= Time.deltaTime;
        }
    }
}