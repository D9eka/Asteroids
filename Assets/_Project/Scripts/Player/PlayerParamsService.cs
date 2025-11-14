using System;
using System.Text;
using _Project.Scripts.Core.InjectIds;
using Asteroids.Scripts.Core;
using Asteroids.Scripts.Weapons.Types.Laser;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Player
{
    public class PlayerParamsService : ITickable, IPlayerParamsService
    {
        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody;
        private readonly ILaserGun _laserGun;
        private readonly ReactiveProperty<string> _params = new ReactiveProperty<string>("");
        
        public IReadOnlyReactiveProperty<string> Params => _params;

        public PlayerParamsService(IPlayerController playerController, 
            [Inject(Id = WeaponInjectId.PlayerLaserGun)] ILaserGun laserGun)
        {
            _transform = playerController.Transform;
            _rigidbody = playerController.Transform.GetComponent<Rigidbody2D>();
            _laserGun = laserGun;
        }

        public void Tick()
        {
            string newText = GenerateText();
            if (_params.Value != newText)
            {
                _params.Value = newText;
            }
        }

        private string GenerateText()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Position: {_transform.position}");
            stringBuilder.AppendLine($"Rotation: {_transform.rotation.eulerAngles}");
            stringBuilder.AppendLine($"Velocity: {_rigidbody.linearVelocity}");
            stringBuilder.AppendLine($"Laser charges: {_laserGun.CurrentCharges}");
            stringBuilder.AppendLine($"Laser cooldown: {_laserGun.ShootCooldown}");
            return stringBuilder.ToString();
        }
    }
}