using System;
using Asteroids.Scripts.Addressable;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectileFactoryInitializer : IInitializable
    {
        private readonly DiContainer _container;
        private readonly IResourcesLoader _resourcesLoader;
        private readonly IProjectileFactory _projectileFactory;

        public ProjectileFactoryInitializer(DiContainer container, 
            IResourcesLoader resourcesLoader, IProjectileFactory projectileFactory)
        {
            _container = container;
            _resourcesLoader = resourcesLoader;
            _projectileFactory = projectileFactory;
        }

        public async void Initialize()
        {
            try
            {
                await CreatePool();
                _projectileFactory.Initialize(_container.Resolve<ProjectilePool>());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async UniTask CreatePool()
        {
            GameObject projectilePrefab = await _resourcesLoader.Load(ResourceObjectId.Projectile);
            _container.BindMemoryPool<Projectile, ProjectilePool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(projectilePrefab.GetComponent<Projectile>())
                .UnderTransformGroup("Projectiles");
            await UniTask.CompletedTask;
        }
    }
}