﻿using Prism.Navigation;
using PingPong.Models;
using Prism.Commands;
using System.Windows.Input;
using PingPong.Views;
using PingPong.Services;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Threading;

namespace PingPong.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ISettingsManager _settingsManager;

        private readonly IGameService _gameService;

        public MainPageViewModel(
            INavigationService navigationService,
            ISettingsManager settingsManager,
            IGameService gameService)
            : base(navigationService)
        {
            _settingsManager = settingsManager;
            _gameService = gameService;

            Game = (GameViewModel)_gameService.GetGame();
        }

        #region -- Public Properties --

        private GameViewModel _game;
        public GameViewModel Game
        {
            get => _game;
            set => SetProperty(ref _game, value);
        }

        private bool _isPresented;
        public bool IsPresented
        {
            get => _isPresented;
            set => SetProperty(ref _isPresented, value);
        }

        public ICommand PlusCommand => new DelegateCommand<string>(OnPlusCommand);

        public ICommand MinusCommand => new DelegateCommand<string>(OnMinusCommand);

        public ICommand NextCommand => new DelegateCommand(OnNextCommand);

        public ICommand GameTappedCommand => new DelegateCommand(OnGameTappedCommand);

        public ICommand OptionsTappedCommand => new DelegateCommand(OnOptionsTappedCommand);

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Game.PointsToWin = _gameService.GetGame().PointsToWin;
        }

        #region -- Private Helpers --

        private void OnNextCommand()
        {
            if (Game.IsNextAllowed)
            {
                if (Game.LeftPoints >= Game.PointsToWin || Game.RightPoints >= Game.PointsToWin)
                {
                    Game.LeftPoints = Game.RightPoints = 0;
                    Game.IsNextAllowed = false;
                    SimpleSpeach.SpeakWithDelay($"Погнали", 0);
                }
            }
        }

        private void OnMinusCommand(string parameter)
        {
            switch (parameter)
            {
                case "Left":
                    if (Game.LeftPoints > 0)
                    {
                        Game.LeftPoints--;
                    }
                    break;

                case "Right":
                    if (Game.RightPoints > 0)
                    {
                        Game.RightPoints--;
                    }
                    break;
            }

            Game.IsNextAllowed = Game.LeftPoints >= Game.PointsToWin || Game.RightPoints >= Game.PointsToWin;

            SimpleSpeach.SpeakWithDelay($"{Game.LeftPoints} : {Game.RightPoints}", 400);
        }

        private async void OnPlusCommand(string parameter)
        {
            switch (parameter)
            {
                case "Left":
                    Game.LeftPoints++;
                    break;

                case "Right":
                    Game.RightPoints++;
                    break;
            }

            Game.IsNextAllowed = Game.LeftPoints >= Game.PointsToWin || Game.RightPoints >= Game.PointsToWin;

            if (Game.IsNextAllowed)
            {
                SimpleSpeach.SpeakWithDelay($"{Game.LeftPoints} : {Game.RightPoints}. Победная!", 400);
            }
            else
            {
                SimpleSpeach.SpeakWithDelay($"{Game.LeftPoints} : {Game.RightPoints}", 400);
            }

        }

        private void OnGameTappedCommand()
        {
            IsPresented = false;
        }
        private async void OnOptionsTappedCommand()
        {
            _gameService.SetGame((GameModel)Game);

            await NavigationService.NavigateAsync(nameof(OptionsPage), null, true, false);
        }

        #endregion
    }

    public static class SimpleSpeach
    {
        private static CancellationTokenSource token;

        public static async void SpeakWithDelay(string text, int miliseconds)
        {
            if (token == null)
            {
                token = new CancellationTokenSource();
            }
            else
            {
                token.Cancel();
                token = new CancellationTokenSource();
            }

            await Task.Delay(miliseconds, token.Token).ContinueWith(r =>
            {
                if (!r.IsCanceled)
                {
                    token = null;
                    TextToSpeech.SpeakAsync(text, new SpeechOptions() { Pitch = 1.3f, Volume = 1 });
                }
            });

        }
    }
}
