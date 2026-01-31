using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Multiplayer.Lobby
{
    public class LobbyUI : MonoBehaviour
    {
        public Action<string> NicknameChanged;
        public Action<string> LobbyCodeChanged;
        public Action HostButtonPressed;
        public Action JoinButtonPressed;
        public Action StartButtonPressed;
    
        [SerializeField] private TMP_InputField _nicknameInput;
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _joinButton;
        [SerializeField] private TextMeshProUGUI _loadingText;
        [SerializeField] private TMP_InputField _lobbyCodeInput;
        [SerializeField] private Button _startGameButton;  
    
        private void Awake()
        {
            _nicknameInput.onValueChanged.AddListener(UpdateNickname);
            _lobbyCodeInput.onValueChanged.AddListener(UpdateLobbyCode);
        
            _hostButton.onClick.AddListener(OnHostClicked);
            _joinButton.onClick.AddListener(OnJoinClicked);
            _startGameButton.onClick.AddListener(OnStartGameClicked);
        
            SetState(LobbyState.Default);
        }

        private void OnDestroy()
        {
            _hostButton.onClick.RemoveListener(OnHostClicked);
            _joinButton.onClick.RemoveListener(OnJoinClicked);
            _startGameButton.onClick.RemoveListener(OnStartGameClicked);
        }

        public void SetLobbyCode(string lobbyCode)
        {
            _lobbyCodeInput.text = lobbyCode;
        }

        public void SetNickname(string nickname)
        {
            _nicknameInput.text = nickname;
        }

        public void SetState(LobbyState state)
        {
            switch (state)
            {
                case LobbyState.Default:
                    _nicknameInput.interactable = true;
                    _lobbyCodeInput.interactable = true;
                    _loadingText.gameObject.SetActive(false);
                    _hostButton.gameObject.SetActive(true);
                    _hostButton.interactable = true;
                    _joinButton.gameObject.SetActive(true);
                    _joinButton.interactable = true;
                    _startGameButton.gameObject.SetActive(false);
                    break;
                case LobbyState.Loading:
                    _nicknameInput.interactable = false;
                    _lobbyCodeInput.interactable = false;
                    _loadingText.gameObject.SetActive(true);
                    _hostButton.gameObject.SetActive(true);
                    _hostButton.interactable = false;
                    _joinButton.gameObject.SetActive(true);
                    _joinButton.interactable = false;
                    _startGameButton.gameObject.SetActive(false);
                    break;
                case LobbyState.Host:
                    _nicknameInput.interactable = false;
                    _lobbyCodeInput.interactable = false;
                    _loadingText.gameObject.SetActive(false);
                    _hostButton.gameObject.SetActive(false);
                    _hostButton.interactable = false;
                    _joinButton.gameObject.SetActive(false);
                    _joinButton.interactable = false;
                    _startGameButton.gameObject.SetActive(true);
                    _startGameButton.interactable = true;
                    break;
                case LobbyState.Client:
                    _nicknameInput.interactable = false;
                    _lobbyCodeInput.interactable = false;
                    _loadingText.gameObject.SetActive(false);
                    _hostButton.gameObject.SetActive(true);
                    _hostButton.interactable = false;
                    _joinButton.gameObject.SetActive(true);
                    _joinButton.interactable = false;
                    _startGameButton.gameObject.SetActive(false);
                    break;
            }
        }

        private void UpdateNickname(string nickname)
        {
            NicknameChanged?.Invoke(nickname);
        }

        private void UpdateLobbyCode(string lobbyCode)
        {
            LobbyCodeChanged?.Invoke(lobbyCode.ToUpper());
        }
    
        private void OnHostClicked()
        {
            HostButtonPressed?.Invoke();
        }

        private void OnJoinClicked()
        {
            JoinButtonPressed?.Invoke();
        }
    
        private void OnStartGameClicked()
        {
            StartButtonPressed?.Invoke();
        }
    }
}
