using Prism.Mvvm;
using Prism.Navigation;

namespace PingPong.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware, IInitialize
    {
        private readonly INavigationService _navigationService;

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public INavigationService NavigationService => _navigationService;

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}
