using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Multiplayer.PlayerListUi
{
    public class PlayerList : MonoBehaviour
    {
        [SerializeField] private GameObject _playerList;
        [SerializeField] private Transform _playerListRoot;
        [SerializeField] private PlayerListItem _playerListItemPrefab;

        public readonly Dictionary<string, PlayerListItem> Players = new();

        public void Add(string nickname, bool isHost, bool isLocal)
        {
            if (!Players.TryGetValue(nickname, out PlayerListItem item))
            {
                item = Instantiate(_playerListItemPrefab, _playerListRoot);
                Players.Add(nickname, item);
            }

            item.SetData(nickname, isHost, isLocal);
            if (isHost)
            {
                item.transform.SetSiblingIndex(0);
            }
        }

        public void Clear()
        {
            foreach (PlayerListItem player in Players.Values)
            {
                Destroy(player.gameObject);
            }
            Players.Clear();
        }
    }
}