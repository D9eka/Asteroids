using System.Text;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Multiplayer.PlayerListUi
{
    public class PlayerListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [Space]
        [SerializeField] private Color _hostColor = Color.yellow;
        [SerializeField] private Color _normalColor = Color.white;

        public void SetData(string nickname, bool isHost, bool isLocal)
        {
            StringBuilder nicknameView = new StringBuilder();
            nicknameView.Append(nickname);
            if (isHost)
            {
                nicknameView.Append(" (Host)");
            }
            if (isLocal)
            {
                nicknameView.Append(" (You)");
            }
            _label.text = nicknameView.ToString();
            _label.color = isHost ? _hostColor : _normalColor;
        }
    }
}