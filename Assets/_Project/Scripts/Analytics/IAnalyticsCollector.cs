using System;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;

namespace Asteroids.Scripts.Analytics
{
    public interface IAnalyticsCollector
    {
        public event Action OnLaserUsed;
        
        public AnalyticsData Analytics { get; }

        public void Initialize(BulletGun playerBulletGun);
        public void Initialize(LaserGun playerLaserGun);
        public void Reset();
    }
}