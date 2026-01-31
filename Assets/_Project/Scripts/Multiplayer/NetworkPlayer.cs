using Fusion;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Multiplayer
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [Networked] public NetworkString<_32> Nickname { get; set; }
        [Networked] public bool IsHost { get; set; }

        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                string nick = PlayerPrefs.GetString("Nickname");
                RpcSetNickname(nick);
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RpcSetNickname(string nick)
        {
            Nickname = nick;
        }

        public string GetNickname() => Nickname.ToString();
    }
}
