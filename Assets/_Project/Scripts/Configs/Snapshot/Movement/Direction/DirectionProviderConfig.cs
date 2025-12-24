using System;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Movement.Direction
{
    [Serializable]
    public class DirectionProviderConfig
    {
        [field: SerializeField] public float MinSpeed { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }

        public DirectionProviderConfig(float minSpeed, float maxSpeed)
        {
            MinSpeed = minSpeed;
            MaxSpeed = maxSpeed;
        }
    }
}