using AMONICAirlinesDesktopApp_Session3.ViewModels;
using System;
using System.Windows;

namespace AMONICAirlinesDesktopApp_Session3.Services
{
    public class WindowService : IWindowService
    {
        public void ShowModalWindow<TViewModel>()
            where TViewModel : BaseViewModel, new()
        {
            Window window = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            TViewModel viewModel = new TViewModel
            {
                CloseAction = new Action(window.Close)
            };
            window.Content = viewModel;
            window.DataContext = viewModel;
            window.Title = viewModel.Title;
            _ = window.ShowDialog();
        }

        public void ShowModalWindowWithParameter<TViewModel, TParam>(
            TParam param)
            where TViewModel : BaseViewModel
        {
            Window window = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            TViewModel viewModel = (TViewModel)Activator
                .CreateInstance(typeof(TViewModel),
                                new object[] { param });
            viewModel.CloseAction = new Action(window.Hide);
            window.Content = viewModel;
            window.DataContext = viewModel;
            window.Title = viewModel.Title;
            _ = window.ShowDialog();
        }

        public void ShowWindow<TViewModel>()
            where TViewModel : BaseViewModel, new()
        {
            Window window = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            TViewModel viewModel = new TViewModel
            {
                CloseAction = new Action(window.Hide)
            };
            window.Content = viewModel;
            window.DataContext = viewModel;
            window.Title = viewModel.Title;
            window.Show();
        }

        public void ShowWindowWithParameter<TViewModel, TParam>(TParam param)
           where TViewModel : BaseViewModel
        {
            Window window = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            TViewModel viewModel = (TViewModel)Activator
                .CreateInstance(typeof(TViewModel),
                                new object[] { param });
            viewModel.CloseAction = new Action(window.Hide);
            window.Content = viewModel;
            window.DataContext = viewModel;
            window.Title = viewModel.Title;
            window.Show();
        }
    }
}
