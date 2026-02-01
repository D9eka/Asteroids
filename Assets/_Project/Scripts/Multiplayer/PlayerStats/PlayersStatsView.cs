using System;
using _Project.Scripts.Multiplayer.PlayerStats;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Multiplayer.PlayerHud
{
    public class PlayersStatsView : MonoBehaviour
    {
        [SerializeField] private PlayerStatsView _localPlayerStatsView;
        [SerializeField] private PlayerStatsView _networkPlayerStatsView;
        
        [Inject] private PlayersStatsViewModel _playersStatsViewModel;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private void Start()
        {
            _playersStatsViewModel.LocalPlayerNickname
                .Subscribe(_localPlayerStatsView.SetPlayerNickname)
                .AddTo(_disposables);

            _playersStatsViewModel.LocalPlayerScore
                .Subscribe(_localPlayerStatsView.SetPlayerScore)
                .AddTo(_disposables);

            _playersStatsViewModel.NetworkPlayerNickname
                .Subscribe(_networkPlayerStatsView.SetPlayerNickname)
                .AddTo(_disposables);

            _playersStatsViewModel.NetworkPlayerScore
                .Subscribe(_networkPlayerStatsView.SetPlayerScore)
                .AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
