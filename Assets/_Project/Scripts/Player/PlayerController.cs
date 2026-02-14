using System;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Player.Weapons;
using UnityEngine;

namespace Asteroids.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public event Action OnKilled;
        
        private IWeaponHandler _weaponHandler;
        
        public int Id  { get; private set; }
        public Transform Transform => transform;

        public void Initialize(IWeaponHandler weaponHandler)
        {
            _weaponHandler = weaponHandler;
        }
        
        public void SetId(int id)
        {
            Id = id;
        }

        public void Attack()
        {
            _weaponHandler.CurrentWeapon?.Shoot();
        }

        public void SwitchWeapon()
        {
            _weaponHandler.SwitchWeapon();
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            OnKilled?.Invoke();
        }
    }
}