using System;
using Asteroids.Scripts.GameState.GameplaySession;
using Asteroids.Scripts.Pause;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Asteroids.Scripts.Audio
{
    public class BackgroundMusicSystem : IPausable, IInitializable, IDisposable
    {
        private readonly IGameplaySessionManager _gameplaySessionManager;
        private readonly IPauseSystem _pauseSystem;
        private readonly AudioSource _audioSource;
        private readonly BackgroundMusicData _backgroundMusicData;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public BackgroundMusicSystem(IGameplaySessionManager gameplaySessionManager, 
            IPauseSystem pauseSystem,
            AudioSource audioSource, BackgroundMusicData backgroundMusicData)
        {
            _gameplaySessionManager = gameplaySessionManager;
            _pauseSystem = pauseSystem;
            _audioSource = audioSource;
            _backgroundMusicData = backgroundMusicData;
        }

        public void Initialize()
        {
            _audioSource.loop = true;
            
            _gameplaySessionManager.GameStarted.Subscribe(_ => PlayMusic())
                .AddTo(_disposables);
            
            _pauseSystem.Register(this);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
        
        private void PlayMusic()
        {
            int musicIndex = Random.Range(0, _backgroundMusicData.BackgroundMusic.Length);
            _audioSource.clip = _backgroundMusicData.BackgroundMusic[musicIndex];
            _audioSource.Play();
        }
        
        public void Pause()
        {
            _audioSource.Pause();
        }
        public void Resume()
        {
            _audioSource.Play();
        }
    }
}