using System;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Audio.Sounds;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Effects
{
    public class BulletGunEffectInitializer : IInitializable
    {
        private readonly DiContainer _container;
        private readonly IResourcesLoader _resourcesLoader;
        private readonly BulletGunEffectFactory _bulletGunEffectFactory;

        public BulletGunEffectInitializer(DiContainer container, IResourcesLoader resourcesLoader, 
            BulletGunEffectFactory bulletGunEffectFactory)
        {
            _container = container;
            _resourcesLoader = resourcesLoader;
            _bulletGunEffectFactory = bulletGunEffectFactory;
        }

        public async void Initialize()
        {
            try
            {
                await CreatePool();
                _bulletGunEffectFactory.Initialize(_container.Resolve<BulletGunEffectPool>());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private async UniTask CreatePool()
        {
            GameObject bulletGunEffect = await _resourcesLoader.Load(ResourceObjectId.BulletGunEffect);
            _container.BindMemoryPool<BulletGunEffect, BulletGunEffectPool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(bulletGunEffect.GetComponent<BulletGunEffect>())
                .UnderTransformGroup("BulletGunEffects");
            await UniTask.CompletedTask;
        }
    }
}