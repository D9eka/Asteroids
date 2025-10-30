using System;

namespace _Project.Scripts.UI
{
    public interface IRestartGameButton
    {
        event Action OnClick;

        void Show();
        void Hide();
    }
}