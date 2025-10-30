using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Score
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        private IScoreService _scoreService;

        [Inject]
        public void Construct(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        private void Awake()
        {
            _scoreService.OnScoreAdded += ScoreServiceOnScoreAdded;
        }

        private void ScoreServiceOnScoreAdded(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}