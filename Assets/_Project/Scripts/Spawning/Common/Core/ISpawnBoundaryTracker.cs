using UnityEngine;

namespace Asteroids.Scripts.Spawning.Common.Core
{
    public interface ISpawnBoundaryTracker
    {
        public void RegisterObject(Transform obj);

        public void UnregisterObject(Transform obj);
    }
}