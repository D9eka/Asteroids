using UnityEngine;

namespace _Project.Scripts.Movement.DirectionProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/LinearDirectionProviderConfig", fileName = "LinearDirectionProviderConfig")]
    public class LinearDirectionProviderConfig : DirectionProviderConfig
    {
        public LinearDirectionProviderConfig()
        {
            DirectionProviderType = DirectionProviderType.Linear;
        }
    }
}