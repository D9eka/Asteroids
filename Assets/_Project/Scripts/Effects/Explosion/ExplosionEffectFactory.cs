using UnityEngine;

namespace Asteroids.Scripts.Effects.Explosion
{
    public class ExplosionEffectFactory
    {
        private ExplosionEffectPool _pool;

        public void Initialize(ExplosionEffectPool pool)
        {
            _pool = pool;
        }
        
        public void Create(Vector3 position, AudioClip clip)
        {
            _pool.Spawn(position, clip);
        }
    }
}