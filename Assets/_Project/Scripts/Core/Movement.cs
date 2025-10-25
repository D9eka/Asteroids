using UnityEngine;

namespace _Project.Scripts.Core
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _thrustPower = 5f;
        [SerializeField] private float _rotationSpeed = 180f;
        
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Move(float input)
        {
            _rb.AddForce(transform.up * (input * _thrustPower));
        }

        public void Rotate(float input)
        {
            _rb.MoveRotation(_rb.rotation - input * _rotationSpeed * Time.fixedDeltaTime);
        }
    }
}