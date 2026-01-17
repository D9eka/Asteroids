using System;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Analytics
{
    public class AnalyticsCollector : IAnalyticsCollector, IInitializable, IDisposable
    {
        public event Action OnLaserUsed;
        
        private readonly IEnemyLifecycleManager _enemyLifecycleManager;
        
        private IWeapon _playerBulletGun;
        private IWeapon _playerLaserGun;
        
        public AnalyticsData Analytics { get; private set; } = new AnalyticsData();
        
        public AnalyticsCollector(IEnemyLifecycleManager enemyLifecycleManager)
        {
            _enemyLifecycleManager = enemyLifecycleManager;
        }
        
        public void Initialize()
        {
            _enemyLifecycleManager.OnEnemyKilled += EnemyLifecycleManagerOnEnemyKilled;
        }
        
        public void Initialize(BulletGun playerBulletGun)
        {
            _playerBulletGun = playerBulletGun;
            _playerBulletGun.OnShoot += PlayerBulletGunOnShoot;
        }
        
        public void Initialize(LaserGun playerLaserGun)
        {
            _playerLaserGun = playerLaserGun;
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

        private void PlayerBulletGunOnShoot(IWeapon weapon)
        {
            Analytics.ShoutsCount++;
        }

        private void PlayerLaserGunOnShoot(IWeapon weapon)
        {
            Analytics.LaserUsageCount++;
            OnLaserUsed?.Invoke();
        }
    }
}