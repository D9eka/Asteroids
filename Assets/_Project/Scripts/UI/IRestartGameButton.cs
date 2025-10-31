using System;

namespace Asteroids.Scripts.UI
{
    public interface IRestartGameButton
    {
        event Action OnClick;

        void Show();
        void Hide();
    }
}