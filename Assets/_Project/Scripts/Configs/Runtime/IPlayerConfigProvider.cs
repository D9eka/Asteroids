using System;
using Asteroids.Scripts.Configs.Snapshot.Player;

namespace Asteroids.Scripts.Configs.Runtime
{
    public interface IPlayerConfigProvider
    {
        public event Action OnConfigUpdated;
        
        public PlayerConfig PlayerConfig { get; }
    }
}