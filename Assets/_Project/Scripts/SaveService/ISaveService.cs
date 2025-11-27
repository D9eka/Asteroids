namespace Asteroids.Scripts.SaveService
{
    public interface ISaveService
    {
        SaveData Load();
        void Save(SaveData saveData);
    }
}