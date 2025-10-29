using _Project.Scripts.Enemies;
using _Project.Scripts.Movement.DirectionProviders;
using _Project.Scripts.Movement.DirectionProviders.Config;
using _Project.Scripts.Movement.RotationProviders;
using _Project.Scripts.Movement.RotationProviders.Config;
using _Project.Scripts.Spawning.Enemies.Config;
using UnityEngine;

namespace _Project.Scripts.Spawning.Enemies.Movement
{
    public interface IEnemyMovementConfigurator
    {
        public void Configure(IEnemy enemy, Vector2 spawnPos, EnemyTypeConfig config);
        public IDirectionProvider CreateDirectionProvider(DirectionProviderConfig parameters, Vector2 direction);
        public IRotationProvider CreateRotationProvider(RotationProviderConfig parameters, Transform self);
    }
}