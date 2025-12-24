using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Movement.Direction
{
    public class TargetDirectionProviderConfig : DirectionProviderConfig
    {
        [field: SerializeField] public float UpdateInterval { get; private set; }

        public TargetDirectionProviderConfig(float updateInterval, float minSpeed, float maxSpeed) : base(minSpeed, maxSpeed)
        {
            UpdateInterval = updateInterval;
        }
    }
}