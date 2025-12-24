using System.Collections.Generic;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.Spawning.Common.Pooling;
using UnityEngine;
using Zenject;
using IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;
using Pooling_IPoolable = Asteroids.Scripts.Spawning.Common.Pooling.IPoolable;

namespace Asteroids.Scripts.WarpSystem
{
    public class BoundsManager : IBoundsManager, ITickable
    {
        private readonly IPoolableLifecycleManager<IPoolable> _lifecycleManager;
        private readonly IBoundsWarp _boundsWarp;
        private readonly float _boundsMargin;
        private readonly List<Transform> _objects = new List<Transform>();

        [Inject]
        public BoundsManager(IPoolableLifecycleManager<IPoolable> lifecycleManager,
            IBoundsWarp boundsWarp, 
            [Inject(Id = FloatInjectId.BoundsMargin)] float boundsMargin)
        {
            _lifecycleManager = lifecycleManager;
            _boundsWarp = boundsWarp;
            _boundsMargin = boundsMargin;
        }

        public void RegisterObject(Transform obj)
        {
            if (!_objects.Contains(obj))
                _objects.Add(obj);
        }

        public void UnregisterObject(Transform obj)
        {
            _objects.Remove(obj);
        }

        public void Tick()
        {
            for (int i = _objects.Count - 1; i >= 0; i--)
            {
                Transform obj = _objects[i];
                if (obj == null)
                {
                    _objects.RemoveAt(i);
                    continue;
                }

                Vector3 pos = obj.position;

                if (IsOutOfBounds(pos))
                {
                    if (obj.TryGetComponent<IWarpable>(out _))
                    {
                        _boundsWarp.WarpObject(obj);
                    }
                    else
                    {
                        if (obj.TryGetComponent(out IPoolable enemy))
                        {
                            _lifecycleManager.Despawn(enemy);
                        }
                        else
                        {
                            Object.Destroy(obj.gameObject);
                        }
                        _objects.RemoveAt(i);
                    }
                }
            }
        }

        public bool IsOutOfBounds(Vector2 pos)
        {
            return pos.x < _boundsWarp.MinBounds.x - _boundsMargin || pos.x > _boundsWarp.MaxBounds.x + _boundsMargin ||
                   pos.y < _boundsWarp.MinBounds.y - _boundsMargin || pos.y > _boundsWarp.MaxBounds.y + _boundsMargin;
        }
    }
}