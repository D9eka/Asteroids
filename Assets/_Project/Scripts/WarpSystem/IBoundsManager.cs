using UnityEngine;

namespace Asteroids.Scripts.WarpSystem
{
    public interface IBoundsManager 
    {
        public void RegisterObject(Transform obj);

        public void UnregisterObject(Transform obj);

        public bool IsOutOfBounds(Vector2 pos);
    }
}