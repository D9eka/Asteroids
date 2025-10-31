using _Project.Scripts.Collision;
using _Project.Scripts.Player;
using _Project.Scripts.Player.Input;
using _Project.Scripts.Player.Movement;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerMovementData _movementData;

        public override void InstallBindings()
        {
            Container.Bind<IPlayerController>().FromInstance(_playerController).AsSingle();
            Container.Bind<ICollisionHandler>()
                .WithId("PlayerCollisionHandler")
                .FromInstance(_playerController.GetComponent<CollisionHandler>())
                .AsCached();
            Container.Bind<PlayerMovementData>().FromInstance(_movementData).AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerMovement>().FromInstance(_movement).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerControllerInitializer>().AsSingle().NonLazy();
        }
    }
}