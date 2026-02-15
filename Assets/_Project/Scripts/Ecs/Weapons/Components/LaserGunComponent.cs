namespace Asteroids.Scripts.Ecs.Weapons.Components
{
    public struct LaserGunComponent
    {
        public int CurrentCharges;
        public int MaxCharges;
        
        public float ShootCooldown;
        public float FireRate;
        
        public float ChargesCooldown;
        public float RechargeRate;
        
        public float ActiveTime;
        public float Duration;
        
        public bool IsActive;
    }
}