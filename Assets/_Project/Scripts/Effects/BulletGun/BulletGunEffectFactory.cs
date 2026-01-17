using UnityEngine;

namespace _Project.Scripts.Effects
{
    public class BulletGunEffectFactory
    {
        private BulletGunEffectPool _pool;

        public void Initialize(BulletGunEffectPool pool)
        {
            _pool = pool;
        }
        
        public void Create(Transform transform)
        {
            _pool.Spawn(transform);
        }
    }
}