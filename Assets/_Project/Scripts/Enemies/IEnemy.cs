using _Project.Scripts.Collision;
using _Project.Scripts.Core;
using _Project.Scripts.Damage;
using _Project.Scripts.Spawning.Common.Pooling;

namespace _Project.Scripts.Enemies
{
    public interface IEnemy : ITransformProvider, IDamageable, IDamageSource, IPoolable
    {
        public event Action<GameObject, IEnemy> OnKilled;
        CollisionHandler CollisionHandler { get; }
        Movement.Core.Movement Movement { get; }
    }
}