using UnityEngine;

namespace Asteroids.Scripts.SaveService
{
    public class PlayerPrefsSaveService : ISaveService
    {
        private const string SAVE_KEY = "player_save";

        public SaveData Load()
        {
            if (!PlayerPrefs.HasKey(SAVE_KEY))
            {
                return new SaveData();
            }

            string json = PlayerPrefs.GetString(SAVE_KEY);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data;
        }

        public void Save(SaveData saveData)
        {
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
        }
    }
}