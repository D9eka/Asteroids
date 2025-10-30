using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class RestartGameButton : MonoBehaviour, IRestartGameButton
    {
        public event Action OnClick;
        
        [SerializeField] private Button _restartButton;

        private void Awake()
        {
            _restartButton.onClick.RemoveAllListeners();
            _restartButton.onClick.AddListener(() => OnClick?.Invoke());
            Hide();
        }
        
        public void Show()
        {
            _restartButton.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _restartButton.gameObject.SetActive(false);
        }
    }
}