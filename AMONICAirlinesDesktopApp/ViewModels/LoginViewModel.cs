using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp.Models.Entities;
using System;
using System.Data.Entity;
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

            Email = "k.omar@amonic.com";
            Password = "4258";
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
            IsNotLoggingIn = false;
            User user = await Task.Run(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    return context.User
                    .Include(u => u.Role)
                    .FirstOrDefault(u =>
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
                (App.Current as App).User = user;
                incorrectLoginAttemps = 0;
                var lastActivity = await Task.Run(() =>
                {
                    using (BaseEntities context = new BaseEntities())
                    {
                        return context.UserActivity
                        .Where(u => u.UserID == user.ID)
                        .ToList()
                        .LastOrDefault();
                    }
                });
                if (lastActivity != null)
                {
                    (App.Current as App).Activity = lastActivity;
                    if (lastActivity.LogoutDateTime == null)
                    {
                        WindowService.ShowModalWindow<TrackingViewModel>();
                        if ((App.Current as App).IsGoToLoginViewModel)
                        {
                            (App.Current as App).IsGoToLoginViewModel = false;
                            return;
                        }
                    }
                }
                var newActivity = new UserActivity
                {
                    LoginDateTime = DateTime.Now,
                    UserID = user.ID,
                };
                (App.Current as App).Activity = newActivity;
                await Task.Run(() =>
                {
                    using (BaseEntities context = new BaseEntities())
                    {
                        _ = context.UserActivity.Add(newActivity);
                        _ = context.SaveChanges();
                    }
                });
                FeedbackService.Inform("Вы авторизованы");
                switch (user.Role.Title)
                {
                    case "Administrator":
                        CloseAction();
                        WindowService
                            .ShowWindow<AdministratorMainMenuViewModel>();
                        break;
                    case "User":
                        CloseAction();
                        WindowService
                            .ShowWindow<UserMainMenuViewModel>();
                        break;
                    default:
                        break;
                }
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
            IsNotLoggingIn = true;
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
        private bool isNotLoggingIn = true;

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
        public bool IsNotLoggingIn
        {
            get => isNotLoggingIn;
            set => SetProperty(ref isNotLoggingIn, value);
        }
    }
}
