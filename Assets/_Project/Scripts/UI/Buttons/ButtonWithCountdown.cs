using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Scripts.UI.Buttons
{
    public class ButtonWithCountdown : Button
    {
        [SerializeField] private float _delaySeconds = 5f;
        [SerializeField] private Image _delayImage;

        private Subject<Unit> _onEndDelay = new Subject<Unit>();
        
        public IObservable<Unit> OnEndDelay => _onEndDelay;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(Delay());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopAllCoroutines();
        }

        private IEnumerator Delay()
        {
            float startedTime = Time.time;

            while (startedTime + _delaySeconds >= Time.time)
            {
                yield return new WaitForEndOfFrame();
                _delayImage.fillAmount = (Time.time - startedTime) / _delaySeconds;
            }
            _onEndDelay.OnNext(Unit.Default);
        }
    }
}