using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using _Project.Scripts.Multiplayer.Pooling;

namespace _Project.Scripts.Multiplayer.Lobby
{
    public class LobbyController : IInitializable
    {
        private const string ROOM_CODE_CHARS = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        
        private readonly NetworkEventsRouter _networkEventsRouter;
        private readonly PlayerSpawner _playerSpawner;
        private readonly LobbyUI _lobbyUI;
        private readonly NetworkRunner _networkRunnerPrefab;
        private readonly SceneRef _gameSceneRef;
        private readonly NetworkObjectPoolRegistry _poolRegistry;
        
        private NetworkRunner _runner;
        private string _nickname;
        private string _lobbyCode;

        public LobbyController(NetworkEventsRouter networkEventsRouter, PlayerSpawner playerSpawner,
            LobbyUI lobbyUI, NetworkRunner networkRunnerPrefab, SceneRef gameSceneRef,
            NetworkObjectPoolRegistry poolRegistry)
        {
            _networkEventsRouter = networkEventsRouter;
            _playerSpawner = playerSpawner;
            _lobbyUI = lobbyUI;
            _networkRunnerPrefab = networkRunnerPrefab;
            _gameSceneRef = gameSceneRef;
            _poolRegistry = poolRegistry;
        }

        public void Initialize()
        {
            _networkEventsRouter.PlayerJoinedEvent += OnPlayerJoined;
            _networkEventsRouter.PlayerLeftEvent += OnPlayerLeft;
            
            _lobbyUI.NicknameChanged += NicknameChanged;
            _lobbyUI.LobbyCodeChanged += LobbyCodeChanged;
            _lobbyUI.HostButtonPressed += HostButtonPressed;
            _lobbyUI.JoinButtonPressed += JoinButtonPressed;
            _lobbyUI.StartButtonPressed += StartButtonPressed;
        }

        private void NicknameChanged(string nickname)
        {
            _nickname = nickname;
        }
        
        private void LobbyCodeChanged(string lobbyCode)
        {
            _lobbyCode = lobbyCode;
        }
        
        private void HostButtonPressed()
        {
            _lobbyCode = GenerateLobbyCode(6);
            _lobbyUI.SetLobbyCode(_lobbyCode);
        
            StartGame(GameMode.Host, _lobbyCode);
        }

        private string GenerateLobbyCode(int length)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = Random.Range(0, ROOM_CODE_CHARS.Length);
                sb.Append(ROOM_CODE_CHARS[index]);
            }
            return sb.ToString();
        }

        private void JoinButtonPressed()
        {
            if (string.IsNullOrWhiteSpace(_lobbyCode))
            {
                Debug.LogWarning("[Lobby] Enter a lobby code before joining.");
                return;
            }
        
            StartGame(GameMode.Client, _lobbyCode);
        }
        
        private void SetLocalNicknameFromInput()
        {
            _nickname = $"Player_{Random.Range(0, 9999)}";
            _lobbyUI.SetNickname(_nickname);
        }
        
        private void StartButtonPressed()
        {
            if (_runner == null)
            {
                Debug.LogWarning("[Lobby] Runner is not started yet.");
                return;
            }

            if (!_runner.IsServer)
            {
                Debug.LogWarning("[Lobby] Only the host can start the game.");
                return;
            }

            if (!_gameSceneRef.IsValid)
            {
                Debug.LogError("[Lobby] Game scene ref is not set. Assign it in the inspector.");
                return;
            }

            Debug.Log("[Lobby] Host pressed 'Start Game'. Loading game scene...");
            _runner.LoadScene(_gameSceneRef);
        }
        
        private async void StartGame(GameMode mode, string sessionName)
        {
            if (_runner != null)
            {
                Debug.LogWarning("Runner already started");
                return;
            }
        
            if (string.IsNullOrEmpty(_nickname))
            {
                SetLocalNicknameFromInput();
            }
            PlayerPrefs.SetString("Nickname", _nickname);

            _lobbyUI.SetState(LobbyState.Loading);

            GameObject runnerGo = Object.Instantiate(_networkRunnerPrefab.gameObject);
            Object.DontDestroyOnLoad(runnerGo);
            _runner = runnerGo.GetComponent<NetworkRunner>();
            _runner.ProvideInput = true;
            _networkEventsRouter.AttachToRunner(_runner);
        
            SceneRef lobbySceneRef = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
            sceneInfo.AddSceneRef(lobbySceneRef);

            NetworkSceneManagerDefault sceneManager = runnerGo.AddComponent<NetworkSceneManagerDefault>();
            NetworkObjectProviderPooled objectProvider = new NetworkObjectProviderPooled(_poolRegistry);

            StartGameResult result = await _runner.StartGame(new StartGameArgs
            {
                GameMode     = mode,
                SessionName  = sessionName,
                Scene        = sceneInfo,
                SceneManager = sceneManager,
                ObjectProvider = objectProvider,
            });

            if (!result.Ok)
            {
                Debug.LogError($"[Lobby] Failed to start runner: {result.ShutdownReason}");
                Object.Destroy(_runner);
                _runner = null;
                return;
            }

            if (_runner.IsServer)
            {
                _lobbyUI.SetState(LobbyState.Host);
            }
            else
            {
                _lobbyUI.SetState(LobbyState.Client);
            }
        }
    
        private void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player joined: {player}");
            _playerSpawner.SpawnPlayer(runner, player);
        }

        private void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player left: {player}");
            _playerSpawner.DespawnPlayer(runner, player);
        }
    }
}
