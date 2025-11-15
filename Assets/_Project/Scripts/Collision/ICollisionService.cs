using UnityEngine;

namespace Asteroids.Scripts.Collision
{
    public interface ICollisionService
    {
        public void OnHit(GameObject origin, GameObject target);
    }
}