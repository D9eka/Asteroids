namespace _Project.Scripts.Movement.Core
{
    public interface IMovable
    {
        public void Stop();
        public void SetVelocity(float velocity);
    }
}