using AMONICAirlinesDesktopApp.Models.Entities;
using AMONICAirlinesDesktopApp.Services;
using AMONICAirlinesDesktopApp.ViewModels;
using System.Windows;

namespace AMONICAirlinesDesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public UserActivity Activity { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DependencyService.Register<MessageBoxFeedbackService>();
            DependencyService.Register<WindowService>();
            DependencyService.Get<IWindowService>()
                .ShowWindow<LoginViewModel>();
        }
    }
}
