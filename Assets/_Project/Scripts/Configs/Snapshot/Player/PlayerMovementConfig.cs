using System;
using UnityEngine;

namespace Asteroids.Scripts.Configs.Snapshot.Player
{
    [Serializable]
    public class PlayerMovementConfig
    {
        [field:SerializeField] public float ThrustForce { get; private set; }
        [field:SerializeField] public float RotationSpeed { get; private set; }

        public PlayerMovementConfig(float thrustForce = 6.5f, float rotationSpeed = 400f)
        {
            ThrustForce = thrustForce;
            RotationSpeed = rotationSpeed;
        }
    }
}