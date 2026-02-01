using TMPro;
using UnityEngine;

namespace _Project.Scripts.Multiplayer.PlayerHud
{
    public class PlayerStatsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private TMP_Text _playerScore;
        
        public void SetPlayerStat(string playerName, int playerScore)
        {
            gameObject.SetActive(true);
            SetPlayerNickname(playerName);
            SetPlayerScore(playerScore);
        }

        public void SetPlayerNickname(string playerNickname)
        {
            gameObject.SetActive(true);
            _playerName.text = playerNickname;
        }

        public void SetPlayerScore(int playerScore)
        {
            gameObject.SetActive(true);
            _playerScore.text = playerScore.ToString();
        }

        public void Clear()
        {
            _playerName.text = string.Empty;
            _playerScore.text = string.Empty;
            gameObject.SetActive(false);
        }
    }
}