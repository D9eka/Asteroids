using _Project.Scripts.Core;
using _Project.Scripts.Weapons;
using Zenject;

namespace _Project.Scripts.Player
{
    public class PlayerControllerInitializer : IInitializable
    {
        private readonly IPlayerController _controller;
        private readonly Movement _movement;
        private readonly IWeapon _weapon;

        public PlayerControllerInitializer(
            IPlayerController controller,
            Movement movement,
            IWeapon weapon)
        {
            _controller = controller;
            _movement = movement;
            _weapon = weapon;
        }

        public void Initialize()
        {
            _controller.Initialize(_movement, _weapon);
        }
    }
}