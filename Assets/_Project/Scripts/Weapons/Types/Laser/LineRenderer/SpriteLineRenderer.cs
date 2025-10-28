using UnityEngine;

namespace _Project.Scripts.Weapons.Types.Laser.LineRenderer
{
    public class SpriteLineRenderer : MonoBehaviour, ILineRenderer
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

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