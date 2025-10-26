using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class Asteroid : MonoBehaviour, IEnemy
    {
        [field: SerializeField] public Movement.Core.Movement Movement { get; private set; }

        public void OnSpawned() => gameObject.SetActive(true);
        public void OnDespawned() => gameObject.SetActive(false);
        public void DestroySelf()
        {
            //TODO: spawn asteroids fragments
            OnDespawned();
        }
    }
}