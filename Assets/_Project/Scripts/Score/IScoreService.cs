using System;
using _Project.Scripts.Enemies;
using UnityEngine;

namespace _Project.Scripts.Score
{
    public interface IScoreService
    {
        public event Action<int> OnScoreAdded;
        
        public void AddScore(GameObject killer, IEnemy enemy);

        public void ResetScore();
    }
}