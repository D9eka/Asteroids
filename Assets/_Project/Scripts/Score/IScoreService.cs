using _Project.Scripts.Enemies;
using UnityEngine;

namespace _Project.Scripts.Score
{
    public interface IScoreService
    {
        void AddScore(GameObject killer, IEnemy enemy);
    }
}