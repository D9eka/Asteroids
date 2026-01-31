using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Multiplayer
{
    public class PlayerSpawner
    {
        private readonly NetworkObject _networkPlayerPrefab;
        
        public readonly Dictionary<PlayerRef, NetworkPlayer> Players;

        public PlayerSpawner(NetworkObject networkPlayerPrefab)
        {
            _networkPlayerPrefab = networkPlayerPrefab;
            Players = new Dictionary<PlayerRef, NetworkPlayer>();
        }

        public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer) return;

            NetworkObject playerObject = runner
                .Spawn(_networkPlayerPrefab, Vector3.zero, Quaternion.identity, player, null, NetworkSpawnFlags.DontDestroyOnLoad);
            NetworkPlayer networkPlayer = playerObject.GetComponent<NetworkPlayer>();
            networkPlayer.IsHost = runner.GameMode == GameMode.Host && player == runner.LocalPlayer;
            Players[player] = networkPlayer;
            runner.SetPlayerObject(player, playerObject);
            Debug.Log("Spawned player: " + player);
        }
        
        public void DespawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer) return;
            
            if (Players.ContainsKey(player))
            {
                runner.Despawn(Players[player].Object);
                Players.Remove(player);
            }
        }
    }
}
