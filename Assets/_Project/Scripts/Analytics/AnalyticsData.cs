namespace Asteroids.Scripts.Analytics
{
    public class AnalyticsData
    {
        public const string SHOUTS_COUNT_PARAMETER_NAME = "shouts";
        public const string LASER_USAGE_PARAMETER_NAME = "laser_usage";
        public const string DESTROYED_ASTEROIDS_COUNT_PARAMETER_NAME = "destroyed_asteroids";
        public const string DESTROYED_UFOS_COUNT_PARAMETER_NAME = "destroyed_ufos";
        
        public int ShoutsCount { get; set; }
        public int LaserUsageCount { get; set; }
        public int DestroyedAsteroidsCount { get; set; }
        public int DestroyedUfosCount { get; set; }
    }
}