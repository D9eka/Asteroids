using Asteroids.Scripts.Ecs.Weapons.Components;
using UniRx;
using UnityEngine;

namespace Asteroids.Scripts.Player
{
    public interface IPlayerParamsService
    {
        public IReadOnlyReactiveProperty<string> Params { get; }

        public void Initialize(Transform playerTransform, Rigidbody2D playerRigidbody, LaserGunComponent laserGunComponent);
    }
}