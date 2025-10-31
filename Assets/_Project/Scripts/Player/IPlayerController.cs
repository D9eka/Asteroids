using System;
using Asteroids.Scripts.Core;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Player.Movement;
using Asteroids.Scripts.Player.Weapons;
using Asteroids.Scripts.WarpSystem;

namespace Asteroids.Scripts.Player
{
    public interface IPlayerController : ITransformProvider, IDamageable, IDamageSource, IWarpable
    {
        public event Action OnKilled;
        
        public void Initialize(IPlayerMovement movement, IWeaponHandler weaponHandler);
        public void SetInputs(float move, float rotate);
        public void Attack();
        public void SwitchWeapon();
    }
}