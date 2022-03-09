using AMONICAirlinesDesktopApp.Services;
using System.Windows;
using System.Windows.Controls;

namespace AMONICAirlinesDesktopApp.Views
{
    /// <summary>
    /// Interaction logic for TrackingView.xaml
    /// </summary>
    public partial class TrackingView : UserControl
    {
        public TrackingView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вызывается в момент загрузки представления.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Closing += (s, args) =>
            {
                if ((App.Current as App).IsSentReason)
                {
                    (App.Current as App).IsSentReason = false;
                    return;
                }
                var feedbackService = DependencyService
                .Get<IFeedbackService>();
                if (feedbackService.Ask("Если вы не укажите причину сбоя, " +
                "то не сможете зайти в систему и вернетесь на окно " +
                "авторизации. Действительно закрыть текущее окно?"))
                {
                    (App.Current as App).IsGoToLoginViewModel = true;
                    App.Current.MainWindow.Visibility = Visibility.Visible;
                }
                else
                {
                    args.Cancel = true;
                }
            };
        }
    }
}
