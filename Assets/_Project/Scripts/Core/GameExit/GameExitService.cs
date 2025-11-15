using UnityEngine;

namespace Asteroids.Scripts.Core.GameExit
{
    public class GameExitService : IGameExitService
    {
        public void Exit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}