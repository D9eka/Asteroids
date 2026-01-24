using System;
using System.Linq;
using Asteroids.Scripts.Configs.Snapshot;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
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

        public void Initialize()
        {
            _firebaseRemoteConfig = FirebaseRemoteConfig.DefaultInstance;
            _firebaseRemoteConfig.OnConfigUpdateListener += ConfigUpdateListenerEventHandler;

            InitializeRemoteConfigAsync().Forget();
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
            config = JsonConvert.DeserializeObject<ConfigData>(configString);
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



        private async UniTask InitializeRemoteConfigAsync()
        {
            try
            {
                await FetchDataAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async UniTask FetchDataAsync()
        {
            Debug.Log("Fetching data...");
            try
            {
                await _firebaseRemoteConfig.FetchAsync(TimeSpan.Zero);
            }
            catch (Exception e)
            {
                await UniTask.SwitchToMainThread();
                Debug.LogException(e);
                return;
            }
            await ApplyFetchedConfigAsync();
        }

        private async UniTask ApplyFetchedConfigAsync()
        {
            await UniTask.SwitchToMainThread();

            ConfigInfo info = _firebaseRemoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError(
                    $"{nameof(FetchDataAsync)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            try
            {
                await _firebaseRemoteConfig.ActivateAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return;
            }
            await UniTask.SwitchToMainThread();
            Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
            OnConfigLoaded?.Invoke();
        }
    }
}