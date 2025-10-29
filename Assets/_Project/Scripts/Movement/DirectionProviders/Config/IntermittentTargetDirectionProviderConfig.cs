using UnityEngine;

namespace _Project.Scripts.Movement.DirectionProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/IntermittentTargetDirectionProviderConfig", fileName = "IntermittentTargetDirectionProviderConfig")]
    public class IntermittentTargetDirectionProviderConfig : DirectionProviderConfig
    {
        [field: SerializeField] public float UpdateInterval { get; private set; }
        [field: SerializeField] public float MoveToTargetChance { get; private set; }

        public IntermittentTargetDirectionProviderConfig()
        {
            DirectionProviderType = DirectionProviderType.IntermittentTarget;
        }
    }
}