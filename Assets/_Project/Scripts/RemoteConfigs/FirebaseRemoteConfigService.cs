using System;
using System.Linq;
using System.Threading.Tasks;
using Asteroids.Scripts.Configs.Snapshot;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.RemoteConfigs
{
    public class FirebaseRemoteConfigService : IRemoteConfigService, IInitializable, IDisposable
    {
        public event Action OnConfigLoaded;
        public event Action OnConfigUpdated;
        
        private const string CONFIG_KEY = "Config";

        private FirebaseRemoteConfig _firebaseRemoteConfig;
        
        public async void Initialize()
        {
            _firebaseRemoteConfig = FirebaseRemoteConfig.DefaultInstance;
            _firebaseRemoteConfig.OnConfigUpdateListener += ConfigUpdateListenerEventHandler;
            
            await FetchDataAsync();
        }
        public void Dispose()
        {
            _firebaseRemoteConfig.OnConfigUpdateListener -= ConfigUpdateListenerEventHandler;
        }

        public bool TryGetConfig(out ConfigData config)
        {
            config = null;
            if (!_firebaseRemoteConfig.Keys.Contains(CONFIG_KEY))
            {
                Debug.LogError($"Key {CONFIG_KEY} does not exist!");
                return false;
            }

            string configString = _firebaseRemoteConfig.GetValue(CONFIG_KEY).StringValue;
            if (string.IsNullOrEmpty(configString))
            {
                Debug.LogError($"Value {CONFIG_KEY} is empty!");
                return false;
            }
            config = JsonUtility.FromJson<ConfigData>(configString);
            return true;
        }

        private void ConfigUpdateListenerEventHandler(object sender, ConfigUpdateEventArgs args) 
        {
            if (args.Error != RemoteConfigError.None) 
            {
                Debug.Log(String.Format("Error occurred while listening: {0}", args.Error));
                return;
            }
            Debug.Log("Updated keys: " + string.Join(", ", args.UpdatedKeys));
            OnConfigUpdated?.Invoke();
        }
        
        private Task FetchDataAsync() {
            Debug.Log("Fetching data...");
            Task fetchTask = _firebaseRemoteConfig.FetchAsync( TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }
        
        private void FetchComplete(Task fetchTask) {
            if (!fetchTask.IsCompleted) {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }

            var info = _firebaseRemoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success) {
                Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            _firebaseRemoteConfig.ActivateAsync()
                .ContinueWithOnMainThread(
                    task => {
                        Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                    });
            OnConfigLoaded?.Invoke();
        }
    }
}