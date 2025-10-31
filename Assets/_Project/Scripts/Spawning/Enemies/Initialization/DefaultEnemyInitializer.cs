﻿using Asteroids.Scripts.Collision;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Enemies.Config;
using Asteroids.Scripts.Pause;
using Asteroids.Scripts.Spawning.Common.Core;
using Asteroids.Scripts.Spawning.Enemies.Movement;

namespace Asteroids.Scripts.Spawning.Enemies.Initialization
{
    public class DefaultEnemyInitializer : EnemyInitializer<IEnemy, EnemyTypeConfig>
    {
        public DefaultEnemyInitializer(ICollisionService collisionService,
            IEnemyMovementConfigurator movementConfigurator, SpawnBoundaryTracker spawnBoundaryTracker, 
            IPauseSystem pauseSystem) : base( collisionService, movementConfigurator, spawnBoundaryTracker, pauseSystem)
        {
        }
    }
}