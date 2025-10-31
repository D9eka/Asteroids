using UnityEngine;

namespace Asteroids.Scripts.Movement.DirectionProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/Movement/Direction/TargetDirectionProviderConfig", fileName = "TargetDirectionProviderConfig")]
    public class TargetDirectionProviderConfig : DirectionProviderConfig
    {
        [field: SerializeField] public float UpdateInterval { get; private set; }

        public TargetDirectionProviderConfig()
        {
            DirectionProviderType = DirectionProviderType.Target;
        }
    }
}