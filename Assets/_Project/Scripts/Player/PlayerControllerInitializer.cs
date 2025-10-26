using _Project.Scripts.WarpSystem;
using _Project.Scripts.Weapons;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player
{
    public class PlayerControllerInitializer : IInitializable
    {
        private readonly IPlayerController _controller;
        private readonly PlayerMovement _movement;
        private readonly IWeapon _weapon;
        private readonly BoundsManager _boundsManager;

        public PlayerControllerInitializer(
            IPlayerController controller,
            PlayerMovement movement,
            IWeapon weapon,
            BoundsManager boundsManager)
        {
            _controller = controller;
            _movement = movement;
            _weapon = weapon;
            _boundsManager = boundsManager;
        }

        public void Initialize()
        {
            _controller.Initialize(_movement, _weapon);
            MonoBehaviour controllerMonoBehaviour = (MonoBehaviour)_controller;
            _boundsManager.RegisterObject(controllerMonoBehaviour.transform);
        }
    }
}