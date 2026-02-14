using System;
using Asteroids.Scripts.Core;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player.Movement;
using Asteroids.Scripts.Player.Weapons;
using Asteroids.Scripts.WarpSystem;

namespace Asteroids.Scripts.Player
{
    public interface IPlayerController : IDamageable, IWarpable
    {
        public event Action OnKilled;
        public void Attack();
        public void SwitchWeapon();
    }
}