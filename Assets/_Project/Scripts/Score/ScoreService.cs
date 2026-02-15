using System;
using System.Collections.Generic;
using Asteroids.Scripts.Configs.Snapshot.Score;
using Asteroids.Scripts.Damage;
using Asteroids.Scripts.Ecs;
using Asteroids.Scripts.Ecs.Colliders.Components;
using Asteroids.Scripts.Enemies;
using Asteroids.Scripts.Spawning.Enemies.Pooling;
using Leopotam.EcsLite;
using UniRx;
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

        public void AddScore(DamageInfo damageInfo, IEnemy enemy)
        {
            if (!CanAddScoreToKiller(damageInfo.InstigatorEntity)) return;

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
            if (!IsEntityAlive(entityId)) return false;
            if (_playerTagPool.Has(entityId)) return true;
            if (!_ownerComponentPool.Has(entityId)) return false;
            int ownerEntity = _ownerComponentPool.Get(entityId).OwnerEntity;
            return IsEntityAlive(ownerEntity) && _playerTagPool.Has(ownerEntity);
        }

        private bool IsEntityAlive(int entityId)
        {
            return entityId >= 0 && _ecsWorld.GetEntityGen(entityId) > 0;
        }
    }
}