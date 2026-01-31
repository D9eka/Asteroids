using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroids.Scripts.Addressable
{
    public interface IResourcesLoader
    {
        UniTask<GameObject> Load(ResourceObjectId resourceObjectId);
    }
}

