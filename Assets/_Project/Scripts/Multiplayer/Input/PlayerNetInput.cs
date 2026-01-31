using Fusion;
using UnityEngine;

namespace _Project.Scripts.Multiplayer.Input
{
    public struct PlayerNetInput : INetworkInput
    {
        public Vector2 Move;
        public NetworkBool Fire;
        public NetworkBool SwitchWeapon;
    }
}

