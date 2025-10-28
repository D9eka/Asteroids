using _Project.Scripts.Collision;
using UnityEngine;

namespace _Project.Scripts.Weapons.Services.Raycast
{
    public interface IRaycastService
    {
        public void Initialize(ICollisionService collisionService, GameObject owner);
        
        public bool TryRaycast(Vector2 origin, Vector2 direction, float maxDistance, out Vector2 hitPoint);
    }
}