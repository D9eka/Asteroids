using Asteroids.Scripts.Movement.DirectionProviders;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Movement.Direction
{
    [CreateAssetMenu(menuName = "Configs/Movement/Direction/TargetDirectionProviderConfig", fileName = "TargetDirectionProviderConfig")]
    public class TargetDirectionProviderConfigSo : DirectionProviderConfigSo
    {
        [field: SerializeField] public float UpdateInterval { get; private set; }

        public TargetDirectionProviderConfigSo()
        {
            DirectionProviderType = Scripts.Movement.DirectionProviders.DirectionProviderType.Target;
        }
    }
}