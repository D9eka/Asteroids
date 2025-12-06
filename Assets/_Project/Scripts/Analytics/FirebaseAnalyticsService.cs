using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Analytics
{
    public class FirebaseAnalyticsService : IAnalyticsService, IInitializable
    {
        private FirebaseApp _firebaseApp;
        private bool _isFirebaseInitialized;

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
        }

        public void SendStartGameEvent()
        {
            if (!_isFirebaseInitialized) return;
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
        }

        public void SendEndGameEvent(AnalyticsData data)
        {
            if (!_isFirebaseInitialized) return;
            FirebaseAnalytics.LogEvent
            (
                FirebaseAnalytics.EventLevelEnd, 
                new Parameter(AnalyticsData.SHOUTS_COUNT_PARAMETER_NAME, data.ShoutsCount),
                new Parameter(AnalyticsData.LASER_USAGE_PARAMETER_NAME, data.LaserUsageCount),
                new Parameter(AnalyticsData.DESTROYED_ASTEROIDS_COUNT_PARAMETER_NAME, data.DestroyedAsteroidsCount),
                new Parameter(AnalyticsData.DESTROYED_UFOS_COUNT_PARAMETER_NAME, data.DestroyedUfosCount)
            );
        }

        public void SendLaserUsedEvent()
        {
            if (!_isFirebaseInitialized) return;
            FirebaseAnalytics.LogEvent("laserUsed");
        }
    }
}