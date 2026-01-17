using System;
using Asteroids.Scripts.Addressable;
using Asteroids.Scripts.Audio.Sounds;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Effects
{
    public class BulletGunEffectInitializer : IInitializable
    {
        private readonly DiContainer _container;
        private readonly IAddressableLoader _addressableLoader;
        private readonly BulletGunEffectFactory _bulletGunEffectFactory;

        public BulletGunEffectInitializer(DiContainer container, IAddressableLoader addressableLoader, 
            BulletGunEffectFactory bulletGunEffectFactory)
        {
            _container = container;
            _addressableLoader = addressableLoader;
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
            GameObject audioSoundPrefab = await _addressableLoader.Load<GameObject>(AddressableId.BulletGunEffect);
            _container.BindMemoryPool<BulletGunEffect, BulletGunEffectPool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(audioSoundPrefab.GetComponent<BulletGunEffect>())
                .UnderTransformGroup("BulletGunEffects");
            await UniTask.CompletedTask;
        }
    }
}