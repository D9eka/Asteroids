using System;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.WarpSystem;

namespace Asteroids.Scripts.Player
{
    public interface IPlayerController : IDamageable, IWarpable
    {
        public event Action OnKilled;
    }
}