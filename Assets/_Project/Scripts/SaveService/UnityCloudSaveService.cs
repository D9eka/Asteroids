using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.SaveService
{
    public class UnityCloudSaveService : ISaveService, IInitializable
    {
        private const string SAVE_KEY = "player_save";
        
        private bool _isStartTryToInitialize = false;
        private bool _isTryToInitialize = false;
        private bool _isInitialized = false;
        
        public void Initialize()
        {
            if (_isStartTryToInitialize) return;
            _isStartTryToInitialize = true;
            SetupAndSignIn().Forget();
        }
        
        private async UniTask SetupAndSignIn()
        {
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                _isInitialized = true;
                Debug.Log($"[UnityCloudSave] Init successful!");
            }
            catch (Exception e)
            {
                Debug.LogError($"[UnityCloudSave] Init failed: {e.Message}");
            }
            finally
            {
                _isTryToInitialize = true;
            }
        }
        
        public async UniTask<SaveData> Load()
        {
            if (!_isStartTryToInitialize)
            {
                Initialize();
            }
            if (!_isTryToInitialize)
            {
                await UniTask.WaitUntil(() => _isTryToInitialize);
            }

            if (!_isInitialized || HasNoInternet())
            {
                return new SaveData();
            }
            
            Dictionary<string, Item> playerData = await 
                CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {
                SAVE_KEY
            });
            
            if (playerData.TryGetValue(SAVE_KEY, out var firstKey))
            {
                string json = firstKey.Value.GetAsString();
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                Debug.Log($"[UnityCloudSave] LOADED! {SAVE_KEY} value: {json}");
                return data;
            }
            return new SaveData();
        }
        
        public async void Save(SaveData saveData)
        {
            if (!_isStartTryToInitialize)
            {
                Initialize();
            }
            if (!_isTryToInitialize)
            {
                await UniTask.WaitUntil(() => _isTryToInitialize);
            }
            
            if (!_isInitialized || HasNoInternet())
            {
                return;
            }
            
            Debug.Log(Application.internetReachability);
            
            saveData.SaveTime = DateTime.UtcNow.Ticks;
            string json = JsonUtility.ToJson(saveData);
            Dictionary<string, object> playerData = new Dictionary<string, object>{
                {SAVE_KEY, json}
            };
            await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
            Debug.Log($"[UnityCloudSave] SAVED! {SAVE_KEY} value: {string.Join(',', playerData)}");
        }

        private bool HasNoInternet()
        {
            return Application.internetReachability == NetworkReachability.NotReachable;
        }
    }
}