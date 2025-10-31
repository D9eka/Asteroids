using Asteroids.Scripts.Input;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Player.Movement
{
    public class PlayerMovementAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _movementImage;
        
        private IPlayerInput _playerInput;
            
        [Inject]
        public void Construct(IPlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        private void Update()
        {
            _movementImage.enabled = _playerInput.Move.y > 0;
        }
    }
}