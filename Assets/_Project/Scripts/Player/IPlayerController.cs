using System;
using Asteroids.Scripts.Damage;

namespace Asteroids.Scripts.Player
{
    public interface IPlayerController : IDamageable
    {
        public event Action OnKilled;
    }
}