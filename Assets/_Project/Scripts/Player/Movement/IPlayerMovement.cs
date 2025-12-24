using Asteroids.Scripts.Configs.Snapshot.Player;
using Asteroids.Scripts.Pause;

namespace Asteroids.Scripts.Player.Movement
{
    public interface IPlayerMovement : IPausable
    {
        public void Initialize(PlayerMovementConfig data);
        
        public void Move(float input);
        public void Rotate(float input);
    }
}