using _Project.Scripts.Collision;
using UnityEngine;

namespace _Project.Scripts.Weapons
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CollisionHandler))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 3f;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rb.linearVelocity = transform.up * speed;
            Destroy(gameObject, lifeTime);
        }
    }
}