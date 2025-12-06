using System;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Analytics
{
    public class AnalyticsController : IAnalyticsController, IInitializable, IDisposable
    {
        private readonly IAnalyticsCollector _analyticsCollector;
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsCollector analyticsCollector, IAnalyticsService analyticsService)
        {
            _analyticsCollector = analyticsCollector;
            _analyticsService = analyticsService;
        }

        public void Initialize()
        {
            _analyticsCollector.OnLaserUsed += SendLaserUsedEvent;
        }

        public void Dispose()
        {
            _analyticsCollector.OnLaserUsed -= SendLaserUsedEvent;
        }

        public void SendStartGameEvent()
        {
            _analyticsService.SendStartGameEvent();
            _analyticsCollector.Reset();
        }

        public void SendEndGameEvent()
        {
            _analyticsService.SendEndGameEvent(_analyticsCollector.Analytics);
        }

        private void SendLaserUsedEvent()
        {
            _analyticsService.SendLaserUsedEvent();
        }
    }
}