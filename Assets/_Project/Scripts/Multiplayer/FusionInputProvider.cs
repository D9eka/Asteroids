using System;
using _Project.Scripts.Multiplayer.Input;
using Asteroids.Scripts.Input;
using Fusion;
using Zenject;

namespace _Project.Scripts.Multiplayer
{
    public class FusionInputProvider : IInitializable, IDisposable
    {
        private readonly NetworkEventsRouter _networkEventsRouter;
        private readonly IPlayerInput _playerInput;

        public FusionInputProvider(NetworkEventsRouter networkEventsRouter, IPlayerInput playerInput)
        {
            _networkEventsRouter = networkEventsRouter;
            _playerInput = playerInput;
        }

        public void Initialize()
        {
            _networkEventsRouter.InputEvent += OnInput;
        }

        public void Dispose()
        {
            _networkEventsRouter.InputEvent -= OnInput;
        }

        private void OnInput(NetworkRunner runner, NetworkInput input)
        {
            PlayerNetInput netInput = new PlayerNetInput
            {
                Move = _playerInput.Move,
                Fire = _playerInput.IsFiring,
                SwitchWeapon = _playerInput.NeedSwitchWeapon
            };

            input.Set(netInput);
            _playerInput.NeedSwitchWeapon = false;
        }
    }
}

