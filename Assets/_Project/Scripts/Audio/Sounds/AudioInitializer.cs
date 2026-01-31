using System;
using Asteroids.Scripts.Addressable;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Audio.Sounds
{
    public class AudioInitializer : IInitializable
    {
        private readonly DiContainer _container;
        private readonly IResourcesLoader _resourcesLoader;
        private readonly AudioSoundFactory _audioSoundFactory;

        public AudioInitializer(DiContainer container, IResourcesLoader resourcesLoader, AudioSoundFactory audioSoundFactory)
        {
            _container = container;
            _resourcesLoader = resourcesLoader;
            _audioSoundFactory = audioSoundFactory;
        }

        public async void Initialize()
        {
            try
            {
                await CreatePool();
                _audioSoundFactory.Initialize(_container.Resolve<AudioSoundPool>());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private async UniTask CreatePool()
        {
            GameObject audioSoundPrefab = await _resourcesLoader.Load(ResourceObjectId.AudioSound);
            _container.BindMemoryPool<AudioSound, AudioSoundPool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(audioSoundPrefab.GetComponent<AudioSound>())
                .UnderTransformGroup("AudioSounds");
            await UniTask.CompletedTask;
        }
    }
}