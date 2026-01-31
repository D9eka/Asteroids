using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace _Project.Scripts.Multiplayer
{
    public class NetworkEventsRouter : MonoBehaviour, INetworkRunnerCallbacks
    {
        public event Action<NetworkRunner> ConnectedToServerEvent;
        public event Action<NetworkRunner, NetDisconnectReason> DisconnectedFromServerEvent;
        public event Action<NetworkRunner, ShutdownReason> ShutdownEvent;
        public event Action<NetworkRunner, NetAddress, NetConnectFailedReason> ConnectFailedEvent;

        public event Action<NetworkRunner, PlayerRef> PlayerJoinedEvent;
        public event Action<NetworkRunner, PlayerRef> PlayerLeftEvent;

        public event Action<NetworkRunner> SceneLoadStartEvent;
        public event Action<NetworkRunner> SceneLoadDoneEvent;

        public event Action<NetworkRunner, NetworkObject, PlayerRef> ObjectEnterAOIEvent;
        public event Action<NetworkRunner, NetworkObject, PlayerRef> ObjectExitAOIEvent;

        public event Action<NetworkRunner, NetworkInput> InputEvent;
        public event Action<NetworkRunner, PlayerRef, NetworkInput> InputMissingEvent;

        public event Action<NetworkRunner, PlayerRef, ReliableKey, ArraySegment<byte>> ReliableDataReceivedEvent;
        public event Action<NetworkRunner, PlayerRef, ReliableKey, float> ReliableDataProgressEvent;

        public event Action<NetworkRunner, SimulationMessagePtr> UserSimulationMessageEvent;

        public event Action<NetworkRunner, NetworkRunnerCallbackArgs.ConnectRequest, byte[]> ConnectRequestEvent;
        public event Action<NetworkRunner, Dictionary<string, object>> CustomAuthenticationResponseEvent;
        public event Action<NetworkRunner, HostMigrationToken> HostMigrationEvent;

        public event Action<NetworkRunner, List<SessionInfo>> SessionListUpdatedEvent;

        private NetworkRunner _attachedRunner;

        public void AttachToRunner(NetworkRunner runner)
        {
            if (runner == null)
            {
                Debug.LogError("[NetworkEventsRouter] Cannot attach to null runner");
                return;
            }

            if (_attachedRunner != null && _attachedRunner != runner)
            {
                Debug.Log("[NetworkEventsRouter] Detaching from previous runner");
                _attachedRunner.RemoveCallbacks(this);
            }

            _attachedRunner = runner;
            _attachedRunner.AddCallbacks(this);
            Debug.Log($"[NetworkEventsRouter] Attached to runner: {runner.name}");
        }

        public void DetachFromRunner()
        {
            if (_attachedRunner == null)
                return;

            _attachedRunner.RemoveCallbacks(this);
            Debug.Log("[NetworkEventsRouter] Detached from runner");
            _attachedRunner = null;
        }

        public NetworkRunner GetAttachedRunner() => _attachedRunner;

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("[NetworkEventsRouter] Connected to server");
            ConnectedToServerEvent?.Invoke(runner);
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            Debug.Log($"[NetworkEventsRouter] Disconnected from server: {reason}");
            DisconnectedFromServerEvent?.Invoke(runner, reason);
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.Log($"[NetworkEventsRouter] Shutdown: {shutdownReason}");
            ShutdownEvent?.Invoke(runner, shutdownReason);
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            Debug.LogError($"[NetworkEventsRouter] Connection failed: {reason} (Address: {remoteAddress})");
            ConnectFailedEvent?.Invoke(runner, remoteAddress, reason);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"[NetworkEventsRouter] Player joined: {player}");
            PlayerJoinedEvent?.Invoke(runner, player);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"[NetworkEventsRouter] Player left: {player}");
            PlayerLeftEvent?.Invoke(runner, player);
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            Debug.Log("[NetworkEventsRouter] Scene load start");
            SceneLoadStartEvent?.Invoke(runner);
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            Debug.Log("[NetworkEventsRouter] Scene load done");
            SceneLoadDoneEvent?.Invoke(runner);
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            ObjectEnterAOIEvent?.Invoke(runner, obj, player);
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            ObjectExitAOIEvent?.Invoke(runner, obj, player);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            InputEvent?.Invoke(runner, input);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            InputMissingEvent?.Invoke(runner, player, input);
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
            ReliableDataReceivedEvent?.Invoke(runner, player, key, data);
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
            ReliableDataProgressEvent?.Invoke(runner, player, key, progress);
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            UserSimulationMessageEvent?.Invoke(runner, message);
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            Debug.Log("[NetworkEventsRouter] Connect request");
            ConnectRequestEvent?.Invoke(runner, request, token);
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            Debug.Log("[NetworkEventsRouter] Custom authentication response");
            CustomAuthenticationResponseEvent?.Invoke(runner, data);
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            HostMigrationEvent?.Invoke(runner, hostMigrationToken);
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            SessionListUpdatedEvent?.Invoke(runner, sessionList);
        }
    }
}
