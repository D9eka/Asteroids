using System;
using Asteroids.Scripts.Addressable;
using _Project.Scripts.Multiplayer.Pooling;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Weapons.Projectile
{
    public class ProjectileFactoryInitializer : IInitializable
    {
        private readonly DiContainer _container;
        private readonly IResourcesLoader _resourcesLoader;
        private readonly IProjectileFactory _projectileFactory;
        private readonly NetworkObjectPoolRegistry _poolRegistry;
        private NetworkObject _projectilePrefab;

        public ProjectileFactoryInitializer(DiContainer container, 
            IResourcesLoader resourcesLoader, IProjectileFactory projectileFactory,
            NetworkObjectPoolRegistry poolRegistry)
        {
            _container = container;
            _resourcesLoader = resourcesLoader;
            _projectileFactory = projectileFactory;
            _poolRegistry = poolRegistry;
        }

        public async void Initialize()
        {
            try
            {
                await CreatePool();
                ProjectilePool pool = _container.Resolve<ProjectilePool>();
                if (_projectilePrefab != null)
                    _poolRegistry?.Register(_projectilePrefab, pool);
                else
                    Debug.LogWarning("[ProjectileFactoryInitializer] Projectile prefab is missing NetworkObject.");

                _projectileFactory.Initialize(_projectilePrefab);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async UniTask CreatePool()
        {
            GameObject projectilePrefab = await _resourcesLoader.Load(ResourceObjectId.Projectile);
            _projectilePrefab = projectilePrefab.GetComponent<NetworkObject>();
            _container.BindMemoryPool<Projectile, ProjectilePool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(projectilePrefab.GetComponent<Projectile>())
                .UnderTransformGroup("Projectiles");
            await UniTask.CompletedTask;
        }
    }
}
