using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp.ViewModels
{
    public class AdministratorMainMenuViewModel : BaseViewModel
    {
        public AdministratorMainMenuViewModel()
        {
            Title = "AMONIC Airlines Automation System";
            var offices = Task.Run(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    return context.Office
                    .ToList();
                }
            }).Result;
            offices.Insert(0,
                           new Office
                           {
                               Title = "All offices"
                           });
            Offices = offices;
            CurrentOffice = Offices.FirstOrDefault();
        }

        private IEnumerable<User> users;

        public IEnumerable<User> Users
        {
            get => users;
            set => SetProperty(ref users, value);
        }

        private IEnumerable<Office> offices;

        public IEnumerable<Office> Offices
        {
            get => offices;
            set => SetProperty(ref offices, value);
        }

        private Office currentOffice;

        public Office CurrentOffice
        {
            get => currentOffice;
            set
            {
                if (SetProperty(ref currentOffice, value))
                {
                    Users = GetUsers();
                }
            }
        }

        /// <summary>
        /// Получает пользователей из базы данных.
        /// </summary>
        /// <returns>Пользователи.</returns>
        private IEnumerable<User> GetUsers()
        {
            IEnumerable<User> currentUsers = App.Current.Dispatcher.Invoke(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    return context.User
                    .Include(u => u.Role)
                    .Include(u => u.Office)
                    .ToList();
                }
            });
            if (CurrentOffice?.Title != "All offices")
            {
                currentUsers = currentUsers
                    .Where(u => u.OfficeID == CurrentOffice.ID)
                    .ToList();
            }
            return currentUsers;
        }

        private User selectedUser;

        public User SelectedUser
        {
            get => selectedUser;
            set => SetProperty(ref selectedUser, value);
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

        private void Exit(object commandParameter)
        {
            if (FeedbackService.Ask("Завершить сессию?"))
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

        private Command toggleActivityCommand;

        public ICommand ToggleActivityCommand
        {
            get
            {
                if (toggleActivityCommand == null)
                {
                    toggleActivityCommand =
                        new Command(ToggleActivity,
                                    (obj) => SelectedUser != null);
                }

                return toggleActivityCommand;
            }
        }

        /// <summary>
        /// Блокирует или разблокирует активность пользователя.
        /// </summary>
        private void ToggleActivity(object commandParameter)
        {
            using (BaseEntities context = new BaseEntities())
            {
                context.User.Find(SelectedUser.ID).Active =
                    !context.User.Find(SelectedUser.ID).Active;
                _ = context.SaveChanges();
            }
            Users = GetUsers();
        }

        private Command addUserCommand;

        public ICommand AddUserCommand
        {
            get
            {
                if (addUserCommand == null)
                {
                    addUserCommand = new Command(AddUser);
                }

                return addUserCommand;
            }
        }

        /// <summary>
        /// Переходит на модель представления 
        /// создания нового пользователя.
        /// </summary>
        private void AddUser(object commandParameter)
        {
            WindowService
                .ShowModalWindowWithParameter
                <AddUserViewModel, User>(new User());
            Users = GetUsers();
        }

        private Command goToChangeRoleViewModelCommand;

        public ICommand GoToChangeRoleViewModelCommand
        {
            get
            {
                if (goToChangeRoleViewModelCommand == null)
                {
                    goToChangeRoleViewModelCommand = new Command(
                        GoToChangeRoleViewModel, (obj) => SelectedUser != null
                        );
                }

                return goToChangeRoleViewModelCommand;
            }
        }

        /// <summary>
        /// Переходит на модель представления 
        /// изменения роли выбранного пользователя.
        /// </summary>
        private void GoToChangeRoleViewModel(object commandParameter)
        {
            WindowService
             .ShowModalWindowWithParameter
             <AddUserViewModel, User>(SelectedUser);
            Users = GetUsers();
        }
    }
}
