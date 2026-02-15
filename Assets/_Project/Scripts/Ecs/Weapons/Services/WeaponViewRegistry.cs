using System.Collections.Generic;
using Asteroids.Scripts.Weapons.Core;

namespace Asteroids.Scripts.Ecs.Weapons.Services
{
    public class WeaponViewRegistry
    {
        private readonly Dictionary<int, IWeapon> _weapons = new Dictionary<int, IWeapon>();
        
        public void Register(int entityId, IWeapon weapon)
        {
            _weapons[entityId] = weapon;
        }

        public void Unregister(int entityId)
        {
            _weapons.Remove(entityId);
        }

        public bool TryGet(int entityId, out IWeapon weapon)
        {
            return _weapons.TryGetValue(entityId, out weapon);
        }
    }
}