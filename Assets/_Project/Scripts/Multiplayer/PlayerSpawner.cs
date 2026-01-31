using Fusion;
using UnityEngine;

namespace _Project.Scripts.Multiplayer
{
    public class PlayerSpawner
    {
        private readonly NetworkObject _networkPlayerPrefab;
        
        public PlayerSpawner(NetworkObject networkPlayerPrefab)
        {
            _networkPlayerPrefab = networkPlayerPrefab;
        }

        public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer) return;

            NetworkObject playerObject = runner
                .Spawn(_networkPlayerPrefab, Vector3.zero, Quaternion.identity, 
                    player, null, NetworkSpawnFlags.DontDestroyOnLoad);
            NetworkPlayer networkPlayer = playerObject.GetComponent<NetworkPlayer>();
            networkPlayer.IsHost = runner.GameMode == GameMode.Host && player == runner.LocalPlayer;
            runner.SetPlayerObject(player, playerObject);
            Debug.Log("Spawned player: " + player);
        }
        
        public void DespawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer) return;
            
            NetworkObject playerObject = runner.GetPlayerObject(player);
            if (playerObject == null)
                return;

            runner.Despawn(playerObject);
        }
    }
}
