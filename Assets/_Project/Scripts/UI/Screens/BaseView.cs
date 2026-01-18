using DG.Tweening;
using UnityEngine;

namespace Asteroids.Scripts.UI.Screens
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        protected virtual void Awake()
        {
            transform.localScale = Vector3.zero;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            gameObject.transform.DOScale(Vector3.one, 0.3f);
        }

        public virtual void Hide()
        {
            gameObject.transform.DOScale(Vector3.zero, 0.3f)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}