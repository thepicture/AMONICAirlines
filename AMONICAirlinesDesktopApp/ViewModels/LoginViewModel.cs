using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp.Models.Entities;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            Title = "Login";
        }

        private Command loginCommand;

        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                {
                    loginCommand = new Command(PerformLoginAsync);
                }

                return loginCommand;
            }
        }

        /// <summary>
        /// Осуществляет авторизацию.
        /// </summary>
        private async void PerformLoginAsync(object commandParameter)
        {
            await Task.Run(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    var user = context.User.FirstOrDefault(u =>
                    u.Email.ToLower()
                    == Email.ToLower());
                    byte[] passwordHash = MD5
                                    .Create()
                                    .ComputeHash(
                                        Encoding.Unicode
                                        .GetBytes(Password + user?.Salt));
                    if (Enumerable.SequenceEqual(user?.PasswordHash, passwordHash))
                    {
                        FeedbackService.Inform("Вы авторизованы");
                    }
                    else
                    {
                        FeedbackService.Warn("Неправильное имя пользователя или пароль");
                    }
                }
            });
        }

        private Command exitCommand;

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new Command(PerformExit);
                }

                return exitCommand;
            }
        }

        /// <summary>
        /// Выключает приложение.
        /// </summary>
        private void PerformExit(object commandParameter)
        {
            if (FeedbackService.Ask("Выйти из приложения?"))
            {
                App.Current.Shutdown();
            }
        }

        private string email;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string password;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
    }
}
