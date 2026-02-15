namespace Asteroids.Scripts.Ecs.Movement.Components
{
    public struct TargetFollowComponent
    {
        public int TargetEntity; 
        public float UpdateInterval; 
        public float LastUpdateTime;
    }
}