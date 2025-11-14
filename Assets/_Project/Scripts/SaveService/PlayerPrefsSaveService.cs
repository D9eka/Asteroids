using UniRx;
using UnityEngine;

namespace Asteroids.Scripts.SaveService
{
    public class PlayerPrefsSaveService : ISaveService
    {
        private const string SAVE_KEY = "player_save";

        public SaveData Data { get; private set; }

        public void Load()
        {
            if (!PlayerPrefs.HasKey(SAVE_KEY))
            {
                Data = new SaveData();
                return;
            }

            string json = PlayerPrefs.GetString(SAVE_KEY);
            var dto = JsonUtility.FromJson<SaveDataDTO>(json);

            Data = new SaveData
            {
                PreviousScore = new ReactiveProperty<int>(dto.PreviousScore),
                HighestScore = new ReactiveProperty<int>(dto.HighestScore)
            };
        }

        public void Persist()
        {
            var dto = new SaveDataDTO
            {
                PreviousScore = Data.PreviousScore.Value,
                HighestScore = Data.HighestScore.Value
            };

            string json = JsonUtility.ToJson(dto);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
        }
    }
}