using Prism.Navigation;
using PingPong.Models;
using Prism.Commands;
using System.Windows.Input;

namespace PingPong.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Game = new GameViewModel();
        }

        #region -- Public Properties --

        private GameViewModel _game;
        public GameViewModel Game
        {
            get => _game;
            set => SetProperty(ref _game, value);
        }

        public ICommand PlusCommand => new DelegateCommand<string>(OnPlusCommand);

        public ICommand MinusCommand => new DelegateCommand<string>(OnMinusCommand);

        public ICommand ClearCommand => new DelegateCommand<string>(OnClearCommand);

        #endregion

        #region -- Private Helpers --

        private void OnClearCommand(string obj)
        {
            Game.CurrentSet.LeftPoints = Game.CurrentSet.RightPoints = 0;
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
        }

        #endregion
    }
}
