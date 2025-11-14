using UniRx;

namespace Asteroids.Scripts.SaveService
{
    public class SaveData
    {
        public ReactiveProperty<int> PreviousScore = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> HighestScore = new ReactiveProperty<int>(0);
    }
    
    [System.Serializable]
    public class SaveDataDTO
    {
        public int PreviousScore;
        public int HighestScore;
    }
}