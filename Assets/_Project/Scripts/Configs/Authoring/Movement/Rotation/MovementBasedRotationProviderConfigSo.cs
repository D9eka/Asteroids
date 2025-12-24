using Asteroids.Scripts.Movement.RotationProviders;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Movement.Rotation
{
    [CreateAssetMenu(menuName = "Configs/Movement/Rotation/MovementBasedRotationProviderConfig", 
        fileName = "MovementBasedRotationProviderConfig")]
    public class MovementBasedRotationProviderConfigSo : RotationProviderConfigSo
    {
        public MovementBasedRotationProviderConfigSo()
        {
            RotationProviderType = Scripts.Movement.RotationProviders.RotationProviderType.MovementBased;
        }
    }
}