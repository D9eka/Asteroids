namespace Asteroids.Scripts.SaveService
{
    [System.Serializable]
    public class SaveData
    {
        public long SaveTime;
        
        public int PreviousScore;
        public int HighestScore;

        public bool IsAdFree;
    }
}