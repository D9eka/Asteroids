namespace Asteroids.Scripts.Analytics
{
    public interface IAnalyticsService
    {
        public void SendStartGameEvent();

        public void SendEndGameEvent(AnalyticsData data);

        public void SendLaserUsedEvent();
    }
}