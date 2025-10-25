using _Project.Scripts.Core;
using _Project.Scripts.Weapons;

namespace _Project.Scripts.Player
{
    public interface IPlayerController : IDestroyable
    {
        public void Initialize(Movement movement, IWeapon weapon);
        public void SetInputs(float move, float rotate);
        public void Attack();
    }
}