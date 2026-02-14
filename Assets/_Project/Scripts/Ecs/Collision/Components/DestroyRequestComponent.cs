using Asteroids.Scripts.Damage;

namespace Asteroids.Scripts.Ecs.Colliders.Components
{
    public struct DestroyRequestComponent
    {
        public DamageType DamageType;
        public int KillerEntity;
    }
}