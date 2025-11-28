using System;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Analytics
{
    public class FirebaseAnalyticsService : IAnalyticsService, IInitializable, IDisposable
    {
        private readonly IAnalyticsCollector _analyticsCollector;

        private FirebaseApp _firebaseApp;
        private bool _isFirebaseInitialized;

        public FirebaseAnalyticsService(IAnalyticsCollector analyticsCollector)
        {
            _analyticsCollector = analyticsCollector;
        }

        public void Initialize()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available) {
                    _firebaseApp = FirebaseApp.DefaultInstance;
                    FirebaseApp.LogLevel = LogLevel.Debug;
                    _isFirebaseInitialized = true;
                } 
                else 
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });

            _analyticsCollector.OnLaserUsed += SendLaserUsedEvent;
        }

        public void Dispose()
        {
            _analyticsCollector.OnLaserUsed -= SendLaserUsedEvent;
        }

        public void SendStartGameEvent()
        {
            if (!_isFirebaseInitialized) return;
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
            _analyticsCollector.Reset();
        }

        public void SendEndGameEvent()
        {
            if (!_isFirebaseInitialized) return;
            FirebaseAnalytics.LogEvent
            (
                FirebaseAnalytics.EventLevelEnd, 
                new Parameter(AnalyticsData.SHOUTS_COUNT_PARAMETER_NAME, _analyticsCollector.Analytics.ShoutsCount),
                new Parameter(AnalyticsData.LASER_USAGE_PARAMETER_NAME, _analyticsCollector.Analytics.LaserUsageCount),
                new Parameter(AnalyticsData.DESTROYED_ASTEROIDS_COUNT_PARAMETER_NAME, 
                    _analyticsCollector.Analytics.DestroyedAsteroidsCount),
                new Parameter(AnalyticsData.DESTROYED_UFOS_COUNT_PARAMETER_NAME, 
                    _analyticsCollector.Analytics.DestroyedUfosCount)
            );
        }

        private void SendLaserUsedEvent()
        {
            if (!_isFirebaseInitialized) return;
            FirebaseAnalytics.LogEvent("laserUsed");
        }
    }
}