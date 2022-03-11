using AMONICAirlinesDesktopApp.Services;
using AMONICAirlinesDesktopApp_Session2.ViewModels;
using System.Windows;

namespace AMONICAirlinesDesktopApp_Session2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DependencyService.Register<MessageBoxFeedbackService>();
            DependencyService.Register<WindowService>();
            DependencyService.Get<IWindowService>()
                .ShowWindow<FlightScheduleViewModel>();
        }
    }
}
