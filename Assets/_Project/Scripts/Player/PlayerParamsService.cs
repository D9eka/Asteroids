using System.Text;
using _Project.Scripts.UI;
using _Project.Scripts.Weapons.Types.Laser;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player
{
    public class PlayerParamsService : ITickable
    {
        private readonly IShowPlayerParams _showPlayerParams;
        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody;
        private readonly ILaserGun _laserGun;

        public PlayerParamsService(IShowPlayerParams showPlayerParams, IPlayerController playerController, [Inject(Id = "PlayerLaserGun")] ILaserGun laserGun)
        {
            _showPlayerParams = showPlayerParams;
            _transform = playerController.Transform;
            _rigidbody = playerController.Transform.GetComponent<Rigidbody2D>();
            _laserGun = laserGun;
        }

        public void Tick()
        {
            _showPlayerParams.Show(GenerateText());
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