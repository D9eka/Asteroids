using UnityEngine;

namespace _Project.Scripts.Input
{
    public interface IPlayerInput
    {
        Vector2 Move { get; }
        bool IsFiring { get; }
        bool NeedSwitchWeapon { get; }
    }
}