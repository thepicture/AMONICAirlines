using AMONICAirlinesDesktopApp_Session3.Models.DistanceFinderModels;
using AMONICAirlinesDesktopApp_Session3.Services;
using AMONICAirlinesDesktopApp_Session3.ViewModels;
using System.Windows;

namespace AMONICAirlinesDesktopApp_Session3
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
            DependencyService.Register<ScheduleDistanceFinder>();

            DependencyService.Get<IWindowService>()
                .ShowWindow<SearchForFlightsViewModel>();
        }
    }
}
