using Asteroids.Scripts.Score;

namespace Asteroids.Scripts.SaveService
{
    public class ScoreSaveHandler : IScoreSaveHandler
    {
        private readonly ISaveService _saveService;
        private readonly IScoreService _scoreService;

        public ScoreSaveHandler(ISaveService saveService, IScoreService scoreService)
        {
            _saveService = saveService;
            _scoreService = scoreService;
        }

        public void SaveCurrentScore()
        {
            var data = _saveService.Data;

            data.PreviousScore.Value = _scoreService.TotalScore.Value;

            if (_scoreService.TotalScore.Value > data.HighestScore.Value)
                data.HighestScore.Value = _scoreService.TotalScore.Value;

            _saveService.Persist();
        }
    }
}