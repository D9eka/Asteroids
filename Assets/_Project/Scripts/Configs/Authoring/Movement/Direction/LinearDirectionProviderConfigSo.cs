using Asteroids.Scripts.Movement.DirectionProviders;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Authoring.Movement.Direction
{
    [CreateAssetMenu(menuName = "Configs/Movement/Direction/LinearDirectionProviderConfig", fileName = "LinearDirectionProviderConfig")]
    public class LinearDirectionProviderConfigSo : DirectionProviderConfigSo
    {
        public LinearDirectionProviderConfigSo()
        {
            DirectionProviderType = DirectionProviderType.Linear;
        }
    }
}