using Asteroids.Scripts.Pause;

namespace Asteroids.Scripts.Player.Movement
{
    public interface IPlayerMovement : IPausable
    {
        public void Initialize(PlayerMovementData data);
        
        public void Move(float input);
        public void Rotate(float input);
    }
}