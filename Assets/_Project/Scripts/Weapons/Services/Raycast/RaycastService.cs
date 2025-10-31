using UnityEngine;

namespace Asteroids.Scripts.Weapons.Services.Raycast
{
    public class RaycastService : IRaycastService
    {
        private GameObject _owner;
        
        public void Initialize(GameObject owner)
        {
            _owner = owner;
        }
        
        public bool TryRaycast(Vector2 origin, Vector2 direction, float maxDistance, out RaycastHit2D hit)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, maxDistance);
            foreach (var hit2D in hits)
            {
                if (hit2D.collider != null && 
                    hit2D.collider.gameObject.activeInHierarchy && hit2D.collider.gameObject != _owner)
                {
                    hit = hit2D;
                    return true;
                }
            }
            
            hit = default;
            return false;
        }
    }
}