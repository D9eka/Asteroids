using System;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using Asteroids.Scripts.Weapons.Core;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Analytics
{
    public class AnalyticsCollector : IAnalyticsCollector, IInitializable, IDisposable
    {
        public event Action OnLaserUsed;
        
        private readonly IEnemyLifecycleManager _enemyLifecycleManager;
        private readonly IWeapon _playerBulletGun;
        private readonly IWeapon _playerLaserGun;
        
        public AnalyticsData Analytics { get; private set; } = new AnalyticsData();
        
        public AnalyticsCollector(
            IEnemyLifecycleManager enemyLifecycleManager,
            [Inject(Id = WeaponInjectId.PlayerBulletGun)] IWeapon playerBulletGun,
            [Inject(Id = WeaponInjectId.PlayerLaserGun)] IWeapon playerLaserGun)
        {
            _enemyLifecycleManager = enemyLifecycleManager;
            _playerBulletGun = playerBulletGun;
            _playerLaserGun = playerLaserGun;
        }
        
        public void Initialize()
        {
            _enemyLifecycleManager.OnEnemyKilled += EnemyLifecycleManagerOnEnemyKilled;
            _playerBulletGun.OnShoot += PlayerBulletGunOnShoot;
            _playerLaserGun.OnShoot += PlayerLaserGunOnShoot;
        }

        public void Dispose()
        {
            _enemyLifecycleManager.OnEnemyKilled -= EnemyLifecycleManagerOnEnemyKilled;
            _playerBulletGun.OnShoot -= PlayerBulletGunOnShoot;
            _playerLaserGun.OnShoot -= PlayerLaserGunOnShoot;
        }
        
        public void Reset()
        {
            Analytics = new AnalyticsData();
        }
        private void EnemyLifecycleManagerOnEnemyKilled(GameObject killer, IEnemy enemy)
        {
            switch (enemy)
            {
                case Asteroid:
                    Analytics.DestroyedAsteroidsCount++;
                    break;
                case Ufo:
                    Analytics.DestroyedUfosCount++;
                    break;
            }
        }

        private void PlayerBulletGunOnShoot()
        {
            Analytics.ShoutsCount++;
        }

        private void PlayerLaserGunOnShoot()
        {
            Analytics.LaserUsageCount++;
            OnLaserUsed?.Invoke();
        }
    }
}