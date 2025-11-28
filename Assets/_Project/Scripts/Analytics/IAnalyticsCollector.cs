using System;

namespace Asteroids.Scripts.Analytics
{
    public interface IAnalyticsCollector
    {
        public event Action OnLaserUsed;
        
        public AnalyticsData Analytics { get; }
        public void Reset();
    }
}