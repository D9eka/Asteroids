using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
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
            SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
            Debug.Log($"[PlayerPrefsSave] LOADED! {SAVE_KEY} value: {json}");
            return data;
        }

        public void Save(SaveData saveData)
        {
            saveData.SaveTime = DateTime.UtcNow.Ticks;
            string json = JsonConvert.SerializeObject(saveData);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
            Debug.Log($"[PlayerPrefsSave] SAVED! {SAVE_KEY} value: {json}");
        }
    }
}