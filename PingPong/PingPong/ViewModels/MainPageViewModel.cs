using Prism.Navigation;
using PingPong.Models;
using Prism.Commands;
using System.Windows.Input;
using System;
using System.Threading.Tasks;
using PingPong.Views;
using PingPong.Services;
using System.Linq;

namespace PingPong.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ISettingsManager _settingsManager;

        public MainPageViewModel(
            INavigationService navigationService,
            ISettingsManager settingsManager)
            : base(navigationService)
        {
            _settingsManager = settingsManager;

            Game = new GameViewModel();
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

        private void OnGameTappedCommand()
        {
            IsPresented = false;
        }

        public ICommand OptionsTappedCommand => new DelegateCommand(OnOptionsTappedCommand);

        private async void OnOptionsTappedCommand()
        {
            await NavigationService.NavigateAsync(nameof(OptionsPage), null, true, false);
        }

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Game.CurrentRule = _settingsManager.GetRules().FirstOrDefault(r => r.IsChoosed);

            var playersAmount = _settingsManager.PlayersAmount;

            for (int i = 0; i < playersAmount; i++)
            {
                Game.Queu.Add(new PlayerViewModel());
            }

            Game.CurrentRule.NextGame(Game);
        }

        #region -- Private Helpers --

        private void OnNextCommand()
        {
            Game.CurrentRule.NextGame(Game);
        }

        private void OnMinusCommand(string parameter)
        {
            switch (parameter)
            {
                case "Left":
                    if (Game.CurrentSet.LeftPoints > 0)
                    {
                        Game.CurrentSet.LeftPoints--;
                    }
                    break;

                case "Right":
                    if (Game.CurrentSet.RightPoints > 0)
                    {
                        Game.CurrentSet.RightPoints--;
                    }
                    break;
            }

            Game.CurrentRule.Update(Game);
        }

        private void OnPlusCommand(string parameter)
        {
            switch (parameter)
            {
                case "Left":
                    Game.CurrentSet.LeftPoints++;
                    break;

                case "Right":
                    Game.CurrentSet.RightPoints++;
                    break;
            }

            Game.CurrentRule.Update(Game);
        }

        #endregion
    }
}
