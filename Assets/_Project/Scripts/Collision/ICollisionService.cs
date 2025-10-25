using UnityEngine;

namespace _Project.Scripts.Collision
{
    public interface ICollisionService
    {
        void HandleCollision(GameObject source, GameObject target);
    }
}