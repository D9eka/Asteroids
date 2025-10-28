using _Project.Scripts.Core;
using _Project.Scripts.Player.Movement;
using _Project.Scripts.Player.Weapons;
using _Project.Scripts.WarpSystem;

namespace _Project.Scripts.Player
{
    public interface IPlayerController : ITransformProvider, IDestroyable, IWarpable
    {
        public void Initialize(IPlayerMovement movement, IWeaponHandler weaponHandler);
        public void SetInputs(float move, float rotate);
        public void Attack();
        public void SwitchWeapon();
    }
}