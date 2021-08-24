using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;
using PingPong.Services;
using PingPong.Models;
using System;

namespace PingPong.ViewModels
{
    public class OptionsPageViewModel : BaseViewModel
    {
        private ISettingsManager _settingsManager;

        private readonly IGameService _gameService;

        public OptionsPageViewModel(
            INavigationService navigationService,
            ISettingsManager settingsManager,
            IGameService gameService)
            : base(navigationService)
        {
            _settingsManager = settingsManager;
            _gameService = gameService;

            Game = (GameViewModel)_gameService.GetGame();
        }

        private GameViewModel _game;
        public GameViewModel Game
        {
            get => _game;
            set => SetProperty(ref _game, value);
        }

        public ICommand GoBackCommand => new DelegateCommand(OnGoBackCommand);

        public ICommand PlusCommand => new DelegateCommand(OnPlusCommand);

        private void OnPlusCommand()
        {
            Game.PointsToWin++;
        }

        public ICommand MinusCommand => new DelegateCommand(OnMinusCommand);

        private void OnMinusCommand()
        {
            Game.PointsToWin--;
        }

        private async void OnGoBackCommand()
        {
            _gameService.SetGame((GameModel)Game);
            _settingsManager.PointsToWin = Game.PointsToWin;
            await NavigationService.GoBackAsync(null, true, true);
        }
    }
}
