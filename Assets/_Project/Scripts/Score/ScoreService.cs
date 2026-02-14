using System;
using System.Collections.Generic;
using Asteroids.Scripts.Configs.Snapshot.Score;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Ecs.Colliders.Components;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using Asteroids.Scripts.Weapons.Projectile;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Score
{
    public class ScoreService : IScoreService, IInitializable, IDisposable
    {
        private readonly EcsWorld _ecsWorld;
        private readonly IEnemyLifecycleManager _enemyLifecycleManager;
        private readonly ReactiveProperty<int> _totalScore = new ReactiveProperty<int>(0);

        private IReadOnlyDictionary<EnemyType, int> _config;
        
        private EcsPool<PlayerTag> _playerTagPool;
        private EcsPool<OwnerComponent> _ownerComponentPool;
        
        public IReadOnlyReactiveProperty<int> TotalScore => _totalScore;

        public ScoreService(EcsWorld ecsWorld, IEnemyLifecycleManager enemyLifecycleManager)
        {
            _ecsWorld = ecsWorld;
            _enemyLifecycleManager = enemyLifecycleManager;
        }

        public void Initialize()
        {
            _playerTagPool = _ecsWorld.GetPool<PlayerTag>();
            _ownerComponentPool = _ecsWorld.GetPool<OwnerComponent>();
            
            _enemyLifecycleManager.OnEnemyKilled += AddScore;
        }

        public void Dispose()
        {
            _enemyLifecycleManager.OnEnemyKilled -= AddScore;
        }
        
        public void ApplyConfig(ScoreConfig scoreConfig)
        {
            _config = scoreConfig.ScoreByConfig;
        }

        public void AddScore(IEcsEntity instigatorEntity, IEnemy enemy)
         {
            if (!CanAddScoreToKiller(instigatorEntity)) return;
            
            int points = CalculatePoints(enemy);
            _totalScore.Value += points;
        }

        public void ResetScore()
        {
            _totalScore.Value = 0;
        }

        private int CalculatePoints(IEnemy enemy)
        {
            return _config[enemy.Type];
        }

        private bool CanAddScoreToKiller(IEcsEntity instigatorEntity)
        {
            if (instigatorEntity == null) return false;
            int entityId = instigatorEntity.Id;
            return _playerTagPool.Has(entityId) || _ownerComponentPool.Has(entityId) && 
                _playerTagPool.Has(_ownerComponentPool.Get(entityId).OwnerEntity);
        }
    }
}