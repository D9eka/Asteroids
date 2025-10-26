using _Project.Scripts.Enemies;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Spawning.Pooling
{
    public class GenericEnemyPool<T> : MonoMemoryPool<Vector3, T> where T : Component, IEnemy
    {
        protected override void Reinitialize(Vector3 position, T item)
        {
            item.transform.position = position;
            item.OnSpawned();
        }

        protected override void OnDespawned(T item)
        {
            base.OnDespawned(item);
            item.OnDespawned();
        }
    }
}