using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroids.Scripts.SaveService
{
    public class PlayerPrefsSaveService : ISaveService
    {
        private const string SAVE_KEY = "player_save";

        public async UniTask<SaveData> Load()
        {
            if (!PlayerPrefs.HasKey(SAVE_KEY))
            {
                return new SaveData();
            }

            string json = PlayerPrefs.GetString(SAVE_KEY);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"[PlayerPrefsSave] LOADED! {SAVE_KEY} value: {json}");
            return data;
        }

        public void Save(SaveData saveData)
        {
            saveData.SaveTime = DateTime.UtcNow.Ticks;
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
            Debug.Log($"[PlayerPrefsSave] SAVED! {SAVE_KEY} value: {json}");
        }
    }
}