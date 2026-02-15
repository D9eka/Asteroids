using System;
using System.Collections.Generic;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Enemies;
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

        public void RemoveEnemy(IEnemy enemy)
        {
            enemy.OnKilled -= PlayExplosionEffect;
            _enemies.Remove(enemy);
        }

        public void Dispose()
        {
            foreach (IEnemy enemy in _enemies)
            {
                enemy.OnKilled -= PlayExplosionEffect;
            }
        }

        private void PlayExplosionEffect(DamageInfo damageInfo, IEnemy enemy)
        {
            if (damageInfo.Type == DamageType.OutOfBounds || damageInfo.Type == DamageType.Timeout)
                return;

            int soundsIndex = Random.Range(0, _explosionSoundData.ExplosionSounds.Length);
            _factory.Create(enemy.Transform.position, _explosionSoundData.ExplosionSounds[soundsIndex]);
        }
    }
}
