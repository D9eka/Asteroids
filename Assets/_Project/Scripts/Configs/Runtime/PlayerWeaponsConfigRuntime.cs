using System;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;

namespace Asteroids.Scripts.Configs.Runtime
{
    public class PlayerWeaponsConfigRuntime : IDisposable
    {
        private readonly IPlayerConfigProvider _playerConfigProvider;
        private IWeapon[] _playerWeapons;

        public PlayerWeaponsConfigRuntime(IPlayerConfigProvider playerConfigProvider)
        {
            _playerConfigProvider = playerConfigProvider;
        }

        public void Initialize(IWeapon[] playerWeapons)
        {
            _playerWeapons = playerWeapons;
            UpdateConfigs();
            _playerConfigProvider.OnPlayerConfigUpdated += UpdateConfigs;
        }
        
        public void Dispose()
        {
            _playerConfigProvider.OnPlayerConfigUpdated -= UpdateConfigs;
        }
        
        private void UpdateConfigs()
        {
            foreach (IWeapon playerWeapon in _playerWeapons)
            {
                if (playerWeapon is BulletGun bulletGun)
                {
                    bulletGun.ApplyConfig(_playerConfigProvider.PlayerConfig.BulletGunConfig);
                    continue;
                }
                if (playerWeapon is LaserGun laser)
                {
                    laser.ApplyConfig(_playerConfigProvider.PlayerConfig.LaserGunConfig);
                }
            }
        }
    }
}