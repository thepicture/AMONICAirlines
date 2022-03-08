using AMONICAirlinesDesktopApp.ViewModels;
using System.Windows;

namespace AMONICAirlinesDesktopApp.Services
{
    public class WindowService : IWindowService
    {
        public void ShowModalWindow<TViewModel>() where TViewModel : BaseViewModel, new()
        {
            Window window = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            TViewModel viewModel = new TViewModel();
            window.Content = viewModel;
            window.DataContext = viewModel;
            window.Title = viewModel.Title;
            viewModel.OnRequestClose += () => window.Close();
            _ = window.ShowDialog();
        }

        public void ShowWindow<TViewModel>()
            where TViewModel : BaseViewModel, new()
        {
            Window window = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            TViewModel viewModel = new TViewModel();
            viewModel.OnRequestClose += () => window.Close();
            window.Content = viewModel;
            window.DataContext = viewModel;
            window.Title = viewModel.Title;
            window.Show();
        }
    }
}
