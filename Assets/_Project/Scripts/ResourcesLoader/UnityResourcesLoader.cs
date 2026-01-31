using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asteroids.Scripts.Addressable
{
    public class UnityResourcesLoader : IResourcesLoader, IDisposable
    {
        private static readonly IReadOnlyDictionary<ResourceObjectId, string> ResourcePaths =
            new Dictionary<ResourceObjectId, string>
            {
                { ResourceObjectId.Player, "Prefabs/Player" },
                { ResourceObjectId.GameplayScreen, "Prefabs/UI/GameplayScreenCanvas" },

                { ResourceObjectId.Projectile, "Prefabs/Weapon/BulletGun/Projectile" },
                { ResourceObjectId.BulletGunEffect, "Prefabs/Weapon/BulletGun/BulletGunEffect" },

                { ResourceObjectId.Asteroid, "Prefabs/Enemies/Asteroid" },
                { ResourceObjectId.AsteroidFragment, "Prefabs/Enemies/AsteroidFragment" },
                { ResourceObjectId.Ufo, "Prefabs/Enemies/UFO" },
                { ResourceObjectId.ExplosionEffect, "Prefabs/Enemies/ExplosionEffect" },

                { ResourceObjectId.AudioSound, "Prefabs/Audio/AudioSound" },
            };

        private readonly Dictionary<ResourceObjectId, UnityEngine.Object> _cache = new();

        public async UniTask<GameObject> Load(ResourceObjectId resourceObjectId)
        {
            if (_cache.TryGetValue(resourceObjectId, out var cached) && cached != null)
            {
                if (cached is GameObject typed)
                {
                    return typed;
                }

                Debug.LogWarning(
                    $"[Resources] Asset '{resourceObjectId}' already loaded as '{cached.GetType().Name}', requested 'GameObject'. Reloading.");
                Unload(resourceObjectId);
            }

            if (!ResourcePaths.TryGetValue(resourceObjectId, out var path) || string.IsNullOrWhiteSpace(path))
            {
                Debug.LogError($"[Resources] No resource path mapped for '{resourceObjectId}'.");
                return default;
            }

            ResourceRequest request = Resources.LoadAsync<GameObject>(path);
            await request.ToUniTask();

            if (request.asset == null)
            {
                Debug.LogError($"[Resources] Failed to load '{resourceObjectId}' at '{path}'. Ensure the asset is under a Resources folder.");
                return default;
            }

            _cache[resourceObjectId] = request.asset;
            return (GameObject)request.asset;
        }

        public void Unload(ResourceObjectId resourceObjectId)
        {
            if (!_cache.TryGetValue(resourceObjectId, out var asset) || asset == null)
            {
                _cache.Remove(resourceObjectId);
                return;
            }

            // Note: Unloading prefab assets is usually safe, but instances must be destroyed separately.
            if (asset is not GameObject && asset is not Component)
            {
                Resources.UnloadAsset(asset);
            }

            _cache.Remove(resourceObjectId);
        }

        public void Dispose()
        {
            foreach (var key in new List<ResourceObjectId>(_cache.Keys))
            {
                Unload(key);
            }
        }
    }
}
