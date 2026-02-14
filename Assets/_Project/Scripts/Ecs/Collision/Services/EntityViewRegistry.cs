using System.Collections.Generic;
using Asteroids.Scripts.Damage;

namespace Asteroids.Scripts.Ecs.Colliders.Services
{
    public class EntityViewRegistry
    {
        private readonly Dictionary<int, IDamageable> _damageables = new Dictionary<int, IDamageable>();
        
        public void Register(int entityId, IDamageable damageable)
        {
            _damageables[entityId] = damageable;
        }

        public void Unregister(int entityId)
        {
            _damageables.Remove(entityId);
        }

        public bool TryGet(int entityId, out IDamageable damageable)
        {
            return _damageables.TryGetValue(entityId, out damageable);
        }
    }
}