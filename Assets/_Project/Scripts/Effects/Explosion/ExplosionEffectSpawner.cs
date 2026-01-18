using System;
using System.Collections.Generic;
using Asteroids.Scripts.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Scripts.Effects.Explosion
{
    public class ExplosionEffectSpawner : IDisposable
    {
        private readonly ExplosionEffectFactory _factory;
        private readonly ExplosionSoundData _explosionSoundData;
        private readonly List<IEnemy> _enemies = new List<IEnemy>();

        public ExplosionEffectSpawner(ExplosionEffectFactory factory,
            ExplosionSoundData explosionSoundData)
        {
            _factory = factory;
            _explosionSoundData = explosionSoundData;
        }
        
        public void AddEnemy(IEnemy enemy)
        {
            _enemies.Add(enemy);
            enemy.OnKilled += PlayExplosionEffect;
        }

        public void Dispose()
        {
            foreach (IEnemy enemy in _enemies)
            {
                enemy.OnKilled -= PlayExplosionEffect;
            }
        }
        
        private void PlayExplosionEffect(GameObject killer, IEnemy enemy)
        {
            int soundsIndex = Random.Range(0, _explosionSoundData.ExplosionSounds.Length);
            _factory.Create(enemy.Transform.position, _explosionSoundData.ExplosionSounds[soundsIndex]);
        }
    }
}