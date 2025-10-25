using UnityEngine;
using Zenject;

namespace _Project.Scripts.Collision
{
    [RequireComponent(typeof(Collider2D))]
    public class CollisionHandler : MonoBehaviour
    {
        [Inject] private ICollisionService _collisionService;

        private void OnTriggerEnter2D(Collider2D other)
        {
            _collisionService.HandleCollision(gameObject, other.gameObject);
        }
    }
}