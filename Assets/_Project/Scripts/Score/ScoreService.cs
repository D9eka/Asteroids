using System.Collections.Generic;
using _Project.Scripts.Enemies;
using _Project.Scripts.Player;
using _Project.Scripts.Spawning.Enemies.Config;
using UnityEngine;

namespace _Project.Scripts.Score
{
    public class ScoreService : IScoreService
    {
        
        private int _totalScore;


        public void AddScore(GameObject killer, IEnemy enemy)
        {
            if (!killer.TryGetComponent<IPlayerController>(out _)) return;
            
            int points = CalculatePoints(killer, enemy);
            _totalScore += points;
            Debug.Log($"[{killer.name}] killed [{enemy.Transform.name}] +{points} pts (Total: {_totalScore})");
        }

        private int CalculatePoints(GameObject killer, IEnemy enemy)
        {
            if (!killer.TryGetComponent<IPlayerController>(out _)) return 0;
        }
    }
}