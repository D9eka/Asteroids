using Asteroids.Scripts.Pause;
using UnityEngine;

namespace Asteroids.Scripts.Input
{
    public interface IPlayerInput : ITickableSystem
    {
        Vector2 Move { get; }
        bool IsFiring { get; }
        bool NeedSwitchWeapon { get; set; }
    }
}