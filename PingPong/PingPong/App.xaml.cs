using Xamarin.Forms;
using Prism;
using Prism.Ioc;
using PingPong.ViewModels;
using PingPong.Views;
using PingPong.Services;

namespace PingPong
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync($"/{nameof(Views.MainPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<OptionsPage, OptionsPageViewModel>();

            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
        }
    }
}
