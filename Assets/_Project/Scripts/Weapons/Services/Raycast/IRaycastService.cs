using UnityEngine;

namespace _Project.Scripts.Weapons.Services.Raycast
{
    public interface IRaycastService
    {
        public void Initialize(GameObject owner);
        
        public bool TryRaycast(Vector2 origin, Vector2 direction, float maxDistance, out RaycastHit2D hit);
    }
}