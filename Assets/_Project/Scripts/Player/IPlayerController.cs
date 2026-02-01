using System;
using _Project.Scripts.Multiplayer;
using Asteroids.Scripts.Core;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Player.Movement;
using Asteroids.Scripts.Player.Weapons;
using Asteroids.Scripts.WarpSystem;
using UniRx;

namespace Asteroids.Scripts.Player
{
    public interface IPlayerController : ITransformProvider, IDamageable, IDamageSource, IWarpable, IPausable
    {
        public event Action<IPlayerController> OnKilled;
        
        public void SetInputs(float move, float rotate);
        public void Attack();
        public void SwitchWeapon();
    }
}
