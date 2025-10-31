using System;
using Asteroids.Scripts.Enemies;
using UnityEngine;

namespace Asteroids.Scripts.Score
{
    public interface IScoreService
    {
        public event Action<int> OnScoreAdded;
        
        public void AddScore(GameObject killer, IEnemy enemy);

        public void ResetScore();
    }
}