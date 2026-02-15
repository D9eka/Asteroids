using System.Text;
using Asteroids.Scripts.Ecs.Weapons.Components;
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
        private LaserGunComponent _laserGunComponent;
        
        public IReadOnlyReactiveProperty<string> Params => _params;

        public void Initialize(Transform playerTransform, Rigidbody2D playerRigidbody, LaserGunComponent laserGunComponent)
        {
            _transform = playerTransform;
            _rigidbody = playerRigidbody;
            _laserGunComponent = laserGunComponent;
        }

        public void Tick()
        {
            if (_transform == null || _rigidbody == null) return;
            
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
            stringBuilder.AppendLine($"Laser charges: {_laserGunComponent.CurrentCharges}");
            stringBuilder.AppendLine($"Laser cooldown: {_laserGunComponent.ShootCooldown}");
            return stringBuilder.ToString();
        }
    }
}