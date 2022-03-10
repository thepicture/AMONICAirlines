using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace AMONICAirlinesDesktopApp.ViewModels
{
    public class UserMainMenuViewModel : BaseViewModel
    {
        public UserMainMenuViewModel()
        {
            Title = "AMONIC Airlines Automation System";
            var userActivities = LoadUserActivitesAsync();
            UserActivities = userActivities
                .Take(
                        userActivities.Count() - 1
                     );
            NumberOfCrashes = userActivities
                .Count(u => u.LogoutDateTime == null) - 1;
            TimeSpentOnSystem = userActivities
                .Where(u => u.LogoutDateTime != null)
                .Select(u => u.TimeSpentOnSystem)
                .Aggregate(TimeSpan.Zero, (t1, t2) =>
                {
                    return t1.Add(t2.Value);
                });
            timer = new DispatcherTimer
                (DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (o, e) =>
            {
                TimeSpentOnSystem += TimeSpan.FromSeconds(1);
            };
            timer.Start();
        }

        /// <summary>
        /// Получает активность текущего пользователя.
        /// </summary>
        private IEnumerable<UserActivity> LoadUserActivitesAsync()
        {
            timer?.Stop();
            int userId = (App.Current as App).User.ID;
            using (BaseEntities context = new BaseEntities())
            {
                return context
                .UserActivity
                .Where(a => a.UserID == userId)
                .ToList();
            }
        }

        private Command exitCommand;

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new Command(Exit);
                }

                return exitCommand;
            }
        }

        /// <summary>
        /// Завершает сессию.
        /// </summary>
        private void Exit(object commandParameter)
        {
            if (FeedbackService.Ask("Действительно завершить сессию?"))
            {
                var activity = (App.Current as App).Activity;
                using (BaseEntities context = new BaseEntities())
                {
                    context.UserActivity
                        .Find(activity.ID)
                        .LogoutDateTime = DateTime.Now;
                    _ = context.SaveChanges();
                }
                CloseAction();
                WindowService.ShowWindow<LoginViewModel>();
            }
        }

        private IEnumerable<UserActivity> userActivities;

        public IEnumerable<UserActivity> UserActivities
        {
            get => userActivities;
            set => SetProperty(ref userActivities, value);
        }

        private TimeSpan timeSpentOnSystem;

        public TimeSpan TimeSpentOnSystem
        {
            get => timeSpentOnSystem;
            set => SetProperty(ref timeSpentOnSystem, value);
        }

        private DispatcherTimer timer;
        private int numberOfCrashes;

        public int NumberOfCrashes
        {
            get => numberOfCrashes;
            set => SetProperty(ref numberOfCrashes, value);
        }
    }
}