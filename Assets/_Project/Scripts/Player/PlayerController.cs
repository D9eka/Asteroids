using System;
using Asteroids.Scripts.Damage;
using UnityEngine;

namespace Asteroids.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public event Action OnKilled;
        
        public int Id  { get; private set; }
        public Transform Transform => transform;
        
        public void SetId(int id)
        {
            Id = id;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            OnKilled?.Invoke();
        }
    }
}