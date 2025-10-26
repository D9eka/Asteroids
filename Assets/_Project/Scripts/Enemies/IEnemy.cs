using _Project.Scripts.Core;

namespace _Project.Scripts.Enemies
{
    public interface IEnemy : IDestroyable
    {
        Movement.Core.Movement Movement { get; }
        void OnSpawned();
        void OnDespawned();
    }
}