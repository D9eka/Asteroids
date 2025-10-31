using UnityEngine;

namespace Asteroids.Scripts.Movement.DirectionProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/Movement/Direction/LinearDirectionProviderConfig", fileName = "LinearDirectionProviderConfig")]
    public class LinearDirectionProviderConfig : DirectionProviderConfig
    {
        public LinearDirectionProviderConfig()
        {
            DirectionProviderType = DirectionProviderType.Linear;
        }
    }
}