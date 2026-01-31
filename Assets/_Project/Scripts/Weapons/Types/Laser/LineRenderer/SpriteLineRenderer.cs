using Fusion;
using UnityEngine;

namespace Asteroids.Scripts.Weapons.Types.Laser.LineRenderer
{
    public class SpriteLineRenderer : NetworkBehaviour, ILineRenderer
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [Networked] private Vector3 NetScale { get; set; }
        
        public override void FixedUpdateNetwork()
        {
            NetScale = transform.localScale;
        }

        public override void Render()
        {
            if (Object.HasStateAuthority)
                return;

            _spriteRenderer.transform.localScale = NetScale;
        }

        public void Enable() => _spriteRenderer.enabled = true;
        public void Disable() => _spriteRenderer.enabled = false;

        public void UpdateLine(Vector2 origin, Vector2 endPosition)
        {
            Vector3 lineScale = _spriteRenderer.transform.localScale;
            float lineHeight = Mathf.Abs(endPosition.y - origin.y);
            _spriteRenderer.transform.localScale = new Vector3(lineScale.x, lineHeight, lineScale.x);
        }
    }
}