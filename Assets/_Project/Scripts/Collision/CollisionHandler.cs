using UnityEngine;

namespace _Project.Scripts.Collision
{
    [RequireComponent(typeof(Collider2D))]
    public class CollisionHandler : MonoBehaviour, ICollisionHandler
    {
        private ICollisionService _collisionService;

        private void OnTriggerEnter2D(Collider2D other)
        {
            _collisionService.OnHit(gameObject, other.gameObject);
        }

        public void Initialize(ICollisionService collisionService)
        {
            _collisionService = collisionService;
        }
    }
}