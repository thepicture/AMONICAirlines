using System.Windows;
using System.Windows.Controls;

namespace AMONICAirlinesDesktopApp.Views
{
    /// <summary>
    /// Interaction logic for AddUserView.xaml
    /// </summary>
    public partial class AddUserView : UserControl
    {
        public AddUserView()
        {
            InitializeComponent();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).Password =
                (sender as PasswordBox).Password;
        }
    }
}
