using UnityEngine;

namespace _Project.Scripts.Movement.RotationProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/MovementBasedRotationProviderConfig", 
        fileName = "MovementBasedRotationProviderConfig")]
    public class MovementBasedRotationProviderConfig : RotationProviderConfig
    {
        public MovementBasedRotationProviderConfig()
        {
            RotationProviderType = RotationProviderType.MovementBased;
        }
    }
}