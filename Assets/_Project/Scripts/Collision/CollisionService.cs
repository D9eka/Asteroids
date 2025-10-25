using _Project.Scripts.Core;
using UnityEngine;

namespace _Project.Scripts.Collision
{
    public class CollisionService : ICollisionService
    {
        public void HandleCollision(GameObject source, GameObject target)
        {
            if (target.TryGetComponent<IDestroyable>(out var destructible))
                destructible.DestroySelf();

            Object.Destroy(source);
        }
    }
}