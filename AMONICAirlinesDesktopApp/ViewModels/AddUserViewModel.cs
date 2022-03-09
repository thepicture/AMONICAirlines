using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp.ViewModels
{
    public class AddUserViewModel : BaseViewModel
    {
        public bool IsInNotCreationMode { get; set; }
        public AddUserViewModel(User user)
        {
            CurrentUser = user;
            Offices = Task.Run(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    return context.Office.ToList();
                }
            }).Result;
            if (user.ID != 0)
            {
                Title = "Edit role";
                AdministratorRole = user.RoleID == 1;
                UserRole = user.RoleID != 1;
                Email = CurrentUser.Email;
                FirstName = CurrentUser.FirstName;
                LastName = CurrentUser.LastName;
                CurrentOffice = Offices.First(o => o.ID == user.OfficeID);
            }
            else
            {
                Title = "Add user";
                IsInNotCreationMode = true;
                CurrentOffice = Offices.FirstOrDefault();
            }
        }

        private string email;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string firstName;

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string lastName;

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private Office currentOffice;

        public Office CurrentOffice
        {
            get => currentOffice;
            set => SetProperty(ref currentOffice, value);
        }

        private DateTime? birthDate;

        public DateTime? BirthDate
        {
            get => birthDate;
            set => SetProperty(ref birthDate, value);
        }

        private IEnumerable<Office> offices;

        public IEnumerable<Office> Offices
        {
            get => offices;
            set => SetProperty(ref offices, value);
        }

        private Command cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new Command(Cancel);
                }

                return cancelCommand;
            }
        }

        /// <summary>
        /// Отменяет создание нового пользователя.
        /// </summary>
        private void Cancel(object commandParameter)
        {
            if (FeedbackService.Ask("Действительно отменить " +
                "создание нового пользователя?"))
            {
                CloseAction();
            }
        }

        private Command saveUserCommand;
        private string errors;
        private string password;

        public ICommand SaveUserCommand
        {
            get
            {
                if (saveUserCommand == null)
                {
                    saveUserCommand = new Command(SaveUser, CanSaveUserExecute);
                }

                return saveUserCommand;
            }
        }

        public string Errors
        {
            get => errors;
            set => SetProperty(ref errors, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public User CurrentUser { get; private set; }

        /// <summary>
        /// Определяет, можно ли сохранить пользователя.
        /// </summary>
        /// <returns><see langword="true"/>, если 
        /// пользователя можно сохранить, 
        /// иначе <see langword="false"/>.</returns>
        private bool CanSaveUserExecute(object arg)
        {
            if (CurrentUser.ID != 0)
            {
                return true;
            }
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Email)
                || email.Length > 150
                || !Regex.IsMatch(Email, @"\w+@\w+\.\w{2,}"))
            {
                _ = builder.AppendLine("Почта - это обязательное поле "
                                       + "длиной до 150 символов "
                                       + "в формате "
                                       + "<логин>"
                                       + "@"
                                       + "<имя домена>"
                                       + "."
                                       + "<расширение домена от 2 символов>");
            }
            if (Task.Run(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    return context
                    .User.Any(u => u.Email.ToLower() == Email.ToLower());
                }
            }).Result)
            {
                _ = builder.AppendLine("Указанный email уже есть. " +
                    "Измените email на другой");
            }
            if (string.IsNullOrWhiteSpace(FirstName) || FirstName.Length > 50)
            {
                _ = builder.AppendLine("Имя - это обязательное поле "
                                   + "длиной до 50 символов");
            }
            if (string.IsNullOrWhiteSpace(LastName) || LastName.Length > 50)
            {
                _ = builder.AppendLine("Фамилия - это обязательное поле "
                                   + "длиной до 50 символов");
            }
            if (CurrentOffice == null)
            {
                _ = builder.AppendLine("Укажите офис пользователя");
            }
            if (!BirthDate.HasValue || BirthDate.Value >= DateTime.Now)
            {
                _ = builder.AppendLine("Дата рождения должна быть " +
                    "меньше текущей даты и является обязательной " +
                    "к заполнению");
            }
            if (string.IsNullOrWhiteSpace(Password) || Password.Length > 100)
            {
                _ = builder.AppendLine("Пароль - это обязательное поле "
                                   + "длиной до 100 символов");
            }
            Errors = builder.Length > 0
                ? builder.ToString()
                : null;
            return builder.Length == 0;
        }

        /// <summary>
        /// Сохраняет пользователя.
        /// </summary>
        private void SaveUser(object commandParameter)
        {
            string salt = Guid
                .NewGuid()
                .ToString();
            byte[] passwordBytes = Encoding
                .Unicode
                .GetBytes(Password + salt);
            byte[] hash = MD5
                .Create()
                .ComputeHash(passwordBytes);
            if (CurrentUser.ID == 0)
            {
                CurrentUser.Email = Email;
                CurrentUser.FirstName = FirstName;
                CurrentUser.LastName = LastName;
                CurrentUser.OfficeID = CurrentOffice.ID;
                CurrentUser.Birthdate = BirthDate;
                CurrentUser.PasswordHash = hash;
                CurrentUser.Salt = salt;
                CurrentUser.Active = true;
                CurrentUser.RoleID = 2;
            }
            else
            {
                CurrentUser.RoleID = (bool)AdministratorRole ? 1 : 2;
            }
            try
            {
                using (BaseEntities context = new BaseEntities())
                {
                    if (CurrentUser.ID == 0)
                    {
                        _ = context.User.Add(CurrentUser);
                    } else
                    {
                        context
                            .User
                            .Find(CurrentUser.ID)
                            .RoleID = CurrentUser.RoleID;
                    }
                    _ = context.SaveChanges();
                }
                CloseAction();
                FeedbackService.Inform("Пользователь успешно сохранён");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                FeedbackService.InformError("Не удалось "
                                            + "сохранить данные пользователя. "
                                            + "Проверьте "
                                            + "подключение к интернету");
            }
        }

        private bool? userRole;

        public bool? UserRole { get => userRole; set => SetProperty(ref userRole, value); }

        private bool? administratorRole;

        public bool? AdministratorRole { get => administratorRole; set => SetProperty(ref administratorRole, value); }
    }
}
