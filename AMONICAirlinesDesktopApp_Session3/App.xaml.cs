using AMONICAirlinesDesktopApp_Session3.Services;
using AMONICAirlinesDesktopApp_Session3.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

            DependencyService.Get<IWindowService>()
                .ShowWindow<SearchForFlightsViewModel>();
        }
    }
}
