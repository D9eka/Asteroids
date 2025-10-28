using _Project.Scripts.Collision;
using _Project.Scripts.Core;
using _Project.Scripts.Spawning.Pooling;

namespace _Project.Scripts.Enemies
{
    public interface IEnemy : IDestroyable, IPoolable
    {
        CollisionHandler CollisionHandler { get; }
        Movement.Core.Movement Movement { get; }
    }
}