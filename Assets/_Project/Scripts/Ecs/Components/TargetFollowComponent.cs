namespace Asteroids.Scripts.Ecs.Components
{
    public struct TargetFollowComponent
    {
        public int TargetEntity; 
        public float UpdateInterval; 
        public float LastUpdateTime;
    }
}