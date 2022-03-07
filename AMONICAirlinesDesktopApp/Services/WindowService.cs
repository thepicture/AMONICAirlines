using AMONICAirlinesDesktopApp.ViewModels;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AMONICAirlinesDesktopApp.Services
{
    public class WindowService : IWindowService
    {
        public void ShowWindow<TViewModel>()
            where TViewModel : BaseViewModel, new()
        {
            Window window = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            TViewModel viewModel = new TViewModel();
            viewModel.OnRequestClose += () => window.Close();
            window.Content = viewModel;
            window.DataContext = viewModel;
            window.Icon = new BitmapImage(
                new Uri($"pack://application:,,,"
                        + "/Resources/DS2017_TP09_color.png"));
            window.Title = viewModel.Title;
            window.Show();
        }
    }
}
