using System;
using System.Threading.Tasks;
using Asteroids.Scripts.Addressable;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectileFactoryInitializer : IInitializable
    {
        private readonly DiContainer _container;
        private readonly IAddressableLoader _addressableLoader;
        private readonly IProjectileFactory _projectileFactory;

        public ProjectileFactoryInitializer(DiContainer container, 
            IAddressableLoader addressableLoader, IProjectileFactory projectileFactory)
        {
            _container = container;
            _addressableLoader = addressableLoader;
            _projectileFactory = projectileFactory;
        }

        public async void Initialize()
        {
            try
            {
                await CreatePool();
                _projectileFactory.Initialize(_container.Resolve<ProjectilePool<Projectile>>());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task CreatePool()
        {
            Task<GameObject> task = _addressableLoader.Load<GameObject>(AddressableId.Projectile);
            await task;
            _container.BindMemoryPool<Projectile, ProjectilePool<Projectile>>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(task.Result.GetComponent<Projectile>())
                .UnderTransformGroup("Projectiles");
            
            await Task.CompletedTask;
        }
    }
}