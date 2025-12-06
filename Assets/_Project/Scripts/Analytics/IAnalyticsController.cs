namespace Asteroids.Scripts.Analytics
{
    public interface IAnalyticsController
    {
        public void SendStartGameEvent();

        public void SendEndGameEvent();
    }
}