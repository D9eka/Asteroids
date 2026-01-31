using System;
using Asteroids.Scripts.Addressable;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Effects.Explosion
{
    public class ExplosionEffectInitializer : IInitializable
    {
        private readonly DiContainer _container;
        private readonly IResourcesLoader _resourcesLoader;
        private readonly ExplosionEffectFactory _explosionEffectFactory;

        public ExplosionEffectInitializer(DiContainer container, IResourcesLoader resourcesLoader, 
            ExplosionEffectFactory explosionEffectFactory)
        {
            _container = container;
            _resourcesLoader = resourcesLoader;
            _explosionEffectFactory = explosionEffectFactory;
        }

        public async void Initialize()
        {
            try
            {
                await CreatePool();
                _explosionEffectFactory.Initialize(_container.Resolve<ExplosionEffectPool>());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private async UniTask CreatePool()
        {
            GameObject explosionEffect = await _resourcesLoader.Load(ResourceObjectId.ExplosionEffect);
            _container.BindMemoryPool<ExplosionEffect, ExplosionEffectPool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(explosionEffect.GetComponent<ExplosionEffect>())
                .UnderTransformGroup("ExplosionEffects");
            await UniTask.CompletedTask;
        }
    }
}