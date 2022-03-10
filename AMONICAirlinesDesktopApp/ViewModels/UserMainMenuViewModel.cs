using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

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
        }

        /// <summary>
        /// Получает активность текущего пользователя.
        /// </summary>
        private IEnumerable<UserActivity> LoadUserActivitesAsync()
        {
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

        private int numberOfCrashes;

        public int NumberOfCrashes
        {
            get => numberOfCrashes;
            set => SetProperty(ref numberOfCrashes, value);
        }
    }
}