using _Project.Scripts.Core;
using UnityEngine;

namespace _Project.Scripts.Collision
{
    public interface ICollisionService
    {
        public void OnHit(GameObject origin, GameObject target);
        public bool CanDestroy(IDestroyable target);

        public bool NeedToDestroySelf(IDestroyable self);
    }
}