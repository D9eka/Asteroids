using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Core.InjectIds;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player.Movement;
using Asteroids.Scripts.Player.Weapons;
using Asteroids.Scripts.WarpSystem;
using Zenject;

namespace Asteroids.Scripts.Player
{
    public class PlayerControllerInitializer : IInitializable
    {
        private readonly IPlayerController _controller;
        private readonly IPlayerMovement _movement;
        private readonly PlayerMovementData _movementData;
        private readonly IWeaponHandler _weaponHandler;
        private readonly BoundsManager _boundsManager;
        private readonly ICollisionService _collisionService;
        private readonly ICollisionHandler _collisionHandler;
        private readonly IPauseSystem _pauseSystem;

        public PlayerControllerInitializer(
            IPlayerController controller,
            IPlayerMovement movement,
            PlayerMovementData movementData,
            IWeaponHandler weaponHandler,
            BoundsManager boundsManager,
            [Inject(Id = CollisionServiceInjectId.Player)] ICollisionService collisionService,
            [Inject(Id = CollisionHandlerInjectId.Player)] ICollisionHandler collisionHandler,
            IPauseSystem pauseSystem)
        {
            _controller = controller;
            _movement = movement;
            _movementData = movementData;
            _weaponHandler = weaponHandler;
            _boundsManager = boundsManager;
            _collisionService = collisionService;
            _collisionHandler = collisionHandler;
            _pauseSystem = pauseSystem;
        }

        public void Initialize()
        {
            _collisionHandler.Initialize(_collisionService);
            _movement.Initialize(_movementData);
            _controller.Initialize(_movement, _weaponHandler);
            _boundsManager.RegisterObject(_controller.Transform);
            _pauseSystem.Register(_controller);
        }
    }
}