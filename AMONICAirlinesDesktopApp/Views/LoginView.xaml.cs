using System.Windows;
using System.Windows.Controls;

namespace AMONICAirlinesDesktopApp.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вызывается в момент изменения пароля.
        /// </summary>
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext)
                .Password = (sender as PasswordBox).Password;
        }
    }
}
