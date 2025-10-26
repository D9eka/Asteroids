using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class Ufo : MonoBehaviour, IEnemy
    {
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }
        
        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);
        public void DestroySelf()
        {
            OnDespawned();
        }
    }
}