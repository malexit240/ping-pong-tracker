using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;
using PingPong.Services;

namespace PingPong.ViewModels
{
    public class OptionsPageViewModel : BaseViewModel
    {
        private ISettingsManager _settingsManager;

        public OptionsPageViewModel(
            INavigationService navigationService,
            ISettingsManager settingsManager)
            : base(navigationService)
        {
            _settingsManager = settingsManager;
        }

        public ICommand GoBackCommand => new DelegateCommand(OnGoBackCommand);

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var rules = _settingsManager.GetRules();
        }

        private async void OnGoBackCommand()
        {
            await NavigationService.GoBackAsync(null, true, true);
        }
    }
}
