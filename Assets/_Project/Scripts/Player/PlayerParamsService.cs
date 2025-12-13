using System.Text;
using Asteroids.Scripts.Weapons.Types.Laser;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Player
{
    public class PlayerParamsService : ITickable, IPlayerParamsService
    {
        private readonly ReactiveProperty<string> _params = new ReactiveProperty<string>("");
        
        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private ILaserGun _laserGun;
        
        public IReadOnlyReactiveProperty<string> Params => _params;

        public void Initialize(Transform playerTransform, Rigidbody2D playerRigidbody, ILaserGun laserGun)
        {
            _transform = playerTransform;
            _rigidbody = playerRigidbody;
            _laserGun = laserGun;
        }

        public void Tick()
        {
            if (_transform == null || _rigidbody == null || _laserGun == null) return;
            
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