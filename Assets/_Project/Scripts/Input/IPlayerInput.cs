using _Project.Scripts.Pause;
using _Project.Scripts.Spawning.Enemies.Core;
using UnityEngine;

namespace _Project.Scripts.Input
{
    public interface IPlayerInput : ITickableSystem
    {
        Vector2 Move { get; }
        bool IsFiring { get; }
        bool NeedSwitchWeapon { get; set; }
    }
}