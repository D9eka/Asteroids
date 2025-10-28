using _Project.Scripts.Core;
using UnityEngine;

namespace _Project.Scripts.Collision
{
    public abstract class CollisionService : ICollisionService
    {
        public void OnHit(GameObject origin, GameObject target)
        {
            if (target.activeInHierarchy && 
                target.TryGetComponent<IDestroyable>(out var targetDestroyable) && CanDestroy(targetDestroyable))
            {
                targetDestroyable.DestroySelf();
            }

            if (target.activeInHierarchy && 
                origin.TryGetComponent<IDestroyable>(out var originDestroyable) && NeedToDestroySelf(originDestroyable))
            {
                originDestroyable.DestroySelf();
            }
        }
        public abstract bool CanDestroy(IDestroyable target);

        public abstract bool NeedToDestroySelf(IDestroyable self);
    }
}