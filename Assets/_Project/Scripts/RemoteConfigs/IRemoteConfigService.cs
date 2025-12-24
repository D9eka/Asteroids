using System;
using Asteroids.Scripts.Configs.Snapshot;

namespace Asteroids.Scripts.RemoteConfigs
{
    public interface IRemoteConfigService
    {
        public event Action OnConfigLoaded;
        public event Action OnConfigUpdated;

        public bool TryGetConfig(out ConfigData config);
    }
}