namespace Asteroids.Scripts.SaveService
{
    public interface ISaveService
    {
        SaveData Data { get; }

        void Load();
        void Persist();
    }
}