using Asteroids.Scripts.Damage;

namespace Asteroids.Scripts.Ecs.Colliders.Components
{
    public struct DamageEventComponent
    {
        public DamageType DamageType;
        public int SourceEntity;
    }
}