using Asteroids.Scripts.Core;

namespace Asteroids.Scripts.Ecs
{
    public interface IEcsEntity : ITransformProvider
    {
        public int Id { get; }
        public void SetId(int id);
    }
}