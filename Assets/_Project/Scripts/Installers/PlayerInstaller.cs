using _Project.Scripts.Player;
using _Project.Scripts.Weapons;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private BulletGun _bulletGun;

        public override void InstallBindings()
        {
            Container.Bind<IPlayerController>().FromInstance(_playerController).AsSingle();
            Container.Bind<PlayerMovement>().FromInstance(_movement).AsSingle();
            Container.Bind<IWeapon>().FromInstance(_bulletGun).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerControllerInitializer>().AsSingle().NonLazy();
        }
    }
}