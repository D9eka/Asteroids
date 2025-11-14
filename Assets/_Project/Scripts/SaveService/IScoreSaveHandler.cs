using Asteroids.Scripts.Score;

namespace Asteroids.Scripts.SaveService
{
    public interface IScoreSaveHandler
    {
        public void SaveCurrentScore();
    }
}