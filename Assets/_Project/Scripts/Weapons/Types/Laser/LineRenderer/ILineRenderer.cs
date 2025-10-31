using UnityEngine;

namespace Asteroids.Scripts.Weapons.Types.Laser.LineRenderer
{
    public interface ILineRenderer
    {
        public void Enable();
        public void Disable();
        public void UpdateLine(Vector2 origin, Vector2 endPosition);
    }
}