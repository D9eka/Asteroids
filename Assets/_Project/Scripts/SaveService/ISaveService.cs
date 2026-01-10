using Cysharp.Threading.Tasks;

namespace Asteroids.Scripts.SaveService
{
    public interface ISaveService
    {
        UniTask<SaveData> Load();
        void Save(SaveData saveData);
    }
}