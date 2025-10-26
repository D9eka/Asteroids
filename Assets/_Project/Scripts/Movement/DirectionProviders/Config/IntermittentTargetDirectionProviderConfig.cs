using UnityEngine;

namespace _Project.Scripts.Movement.DirectionProviders.Config
{
    [CreateAssetMenu(menuName = "Configs/IntermittentTargetDirectionProviderConfig", fileName = "IntermittentTargetDirectionProviderConfig")]
    public class IntermittentTargetDirectionProviderConfig : DirectionProviderConfig
    {
        [field: SerializeField] public float UpdateInterval { get; set; } = 2f;
        [field: SerializeField] public float MoveToTargetChance { get; set; } = 0.75f;

        public IntermittentTargetDirectionProviderConfig()
        {
            DirectionProviderType = DirectionProviderType.IntermittentTarget;
        }
    }
}