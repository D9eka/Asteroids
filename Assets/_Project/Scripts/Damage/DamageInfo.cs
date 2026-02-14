using Asteroids.Scripts.Ecs;

namespace Asteroids.Scripts.Damage
{
    public class DamageInfo
    {
        public DamageType Type { get; }
        public IEcsEntity InstigatorEntity { get; }

        public DamageInfo(DamageType type, IEcsEntity instigatorEntity = null)
        {
            Type = type;
            InstigatorEntity = instigatorEntity;
        }
    }
}