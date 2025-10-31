﻿using System.Collections.Generic;
using Asteroids.Scripts.Spawning.Common.Pooling;
using Asteroids.Scripts.WarpSystem;
using UnityEngine;
using Zenject;
using IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.Spawning.Common.Core
{
    public class SpawnBoundaryTracker : IFixedTickable
    {
        private const float MAX_REGISTRATION_TIME_SECONDS = 5f;

        private readonly IPoolableLifecycleManager<Pooling_IPoolable> _lifecycleManager;
        private readonly IBoundsManager _boundsManager;
        private readonly Dictionary<Transform, float> _objects = new Dictionary<Transform, float>();

        [Inject]
        public SpawnBoundaryTracker(IPoolableLifecycleManager<Pooling_IPoolable> lifecycleManager, IBoundsManager boundsManager)
        {
            _lifecycleManager = lifecycleManager;
            _boundsManager = boundsManager;
        }

        public void RegisterObject(Transform obj)
        {
            if (!_objects.ContainsKey(obj))
            {
                _objects.Add(obj, Time.time);
                _boundsManager.UnregisterObject(obj);
            }
        }

        public void UnregisterObject(Transform obj)
        {
            _objects.Remove(obj);
        }

        public void FixedTick()
        {
            List<Transform> objectsToRemove = new List<Transform>();

            foreach (var pair in _objects)
            {
                Transform obj = pair.Key;
                float registrationTime = pair.Value;

                if (obj == null)
                {
                    objectsToRemove.Add(obj);
                    continue;
                }

                if (Time.time - registrationTime > MAX_REGISTRATION_TIME_SECONDS)
                {
                    DestroyObject(obj);
                    objectsToRemove.Add(obj);
                    continue;
                }

                if (!_boundsManager.IsOutOfBounds(obj.position))
                {
                    _boundsManager.RegisterObject(obj);
                    objectsToRemove.Add(obj);
                }
            }

            foreach (var obj in objectsToRemove)
            {
                UnregisterObject(obj);
            }
        }

        private void DestroyObject(Transform obj)
        {
            if (obj.TryGetComponent(out Pooling_IPoolable poolable))
            {
                _lifecycleManager.Despawn(poolable);
            }
            else
            {
                Object.Destroy(obj.gameObject);
            }
        }
    }
}