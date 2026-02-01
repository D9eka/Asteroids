using Fusion;
using UnityEngine;

namespace _Project.Scripts.Multiplayer.Pooling
{
    public interface IPoolAdapter
    {
        NetworkObject Spawn(Vector3 position, Quaternion rotation);
        void Despawn(NetworkObject instance);
    }
}