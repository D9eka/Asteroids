using _Project.Scripts.Collision;
using UnityEngine;

namespace _Project.Scripts.Weapons.Services.Raycast
{
    public class RaycastService : IRaycastService
    {
        private ICollisionService _collisionService;
        private GameObject _owner;
        
        public void Initialize(ICollisionService collisionService, GameObject owner)
        {
            _collisionService = collisionService;
            _owner = owner;
        }
        
        public bool TryRaycast(Vector2 origin, Vector2 direction, float maxDistance, out Vector2 hitPoint)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, maxDistance);
            foreach (var hit in hits)
            {
                if (hit.collider != null && 
                    hit.collider.gameObject.activeInHierarchy && hit.collider.gameObject != _owner)
                {
                    _collisionService.OnHit(_owner, hit.collider.gameObject);
                    hitPoint = hit.point;
                    return true;
                }
            }
            hitPoint = origin + direction * maxDistance;
            Debug.DrawRay(origin, direction * maxDistance, Color.cyan);
            return false;
        }
    }
}