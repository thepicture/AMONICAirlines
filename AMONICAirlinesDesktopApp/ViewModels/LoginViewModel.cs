using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp.Models.Entities;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

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
            User user = await Task.Run(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    return context.User.FirstOrDefault(u =>
                    u.Email.ToLower()
                    == Email.ToLower());
                }
            });
            byte[] passwordHash = MD5
                            .Create()
                            .ComputeHash(
                                Encoding.Unicode
                                .GetBytes(Password + user?.Salt));
            if (user != null
            && Enumerable.SequenceEqual(user.PasswordHash,
                                         passwordHash))
            {
                if (user.Active != null && (bool)!user.Active)
                {
                    string reason = user.InactiveReason ?? "неизвестна";
                    FeedbackService.Warn("Вход невозможен. " +
                        "Руководство заблокировало ваш аккаунт. Причина: " +
                        $"{reason}");
                    return;
                }
                incorrectLoginAttemps = 0;
                FeedbackService.Inform("Вы авторизованы");
            }
            else
            {
                incorrectLoginAttemps++;
                if (incorrectLoginAttemps > 3)
                {
                    PerformWaitBeforeNextLogin();
                }
                FeedbackService.Warn("Неправильное "
                                     + "имя пользователя "
                                     + "или пароль");
            }
        }

        private TimeSpan awaitTime = TimeSpan.FromSeconds(10);
        private TimeSpan currentTimeSpan;

        /// <summary>
        /// Устанавливает блокировку ввода на 10 секунд 
        /// прежде, чем пользователь снова сможет зайти в систему.
        /// </summary>
        private void PerformWaitBeforeNextLogin()
        {
            CurrentTimeSpan = awaitTime;
            DispatcherTimer timer = new DispatcherTimer(
                DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            timer.Tick += (s, e) =>
            {
                CurrentTimeSpan = CurrentTimeSpan.Subtract(timer.Interval);
                if (CurrentTimeSpan == TimeSpan.Zero)
                {
                    timer.Stop();
                    IsBusy = false;
                }
            };
            IsBusy = true;
            timer.Start();
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
        private int incorrectLoginAttemps;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public TimeSpan CurrentTimeSpan
        {
            get => currentTimeSpan;
            set => SetProperty(ref currentTimeSpan, value);
        }
    }
}
