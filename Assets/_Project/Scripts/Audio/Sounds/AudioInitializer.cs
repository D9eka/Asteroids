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
        private readonly IAddressableLoader _addressableLoader;
        private readonly AudioSoundFactory _audioSoundFactory;

        public AudioInitializer(DiContainer container, IAddressableLoader addressableLoader, AudioSoundFactory audioSoundFactory)
        {
            _container = container;
            _addressableLoader = addressableLoader;
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
            GameObject audioSoundPrefab = await _addressableLoader.Load<GameObject>(AddressableId.AudioSound);
            _container.BindMemoryPool<AudioSound, AudioSoundPool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(audioSoundPrefab.GetComponent<AudioSound>())
                .UnderTransformGroup("AudioSounds");
            await UniTask.CompletedTask;
        }
    }
}