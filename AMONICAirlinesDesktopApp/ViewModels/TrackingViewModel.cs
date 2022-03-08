using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp.Models.Entities;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp.ViewModels
{
    public class TrackingViewModel : BaseViewModel
    {
        public TrackingViewModel()
        {
            Title = "No logout detected";
            LastLogin = (App.Current as App).Activity.LoginDateTime;
        }

        private DateTime lastLogin;

        public DateTime LastLogin
        {
            get => lastLogin;
            set => SetProperty(ref lastLogin, value);
        }

        private bool? softwareCrash;

        public bool? SoftwareCrash
        {
            get => softwareCrash;
            set => SetProperty(ref softwareCrash, value);
        }

        private bool? systemCrash;

        public bool? SystemCrash
        {
            get => systemCrash;
            set => SetProperty(ref systemCrash, value);
        }

        private Command confirmCommand;

        public ICommand ConfirmCommand
        {
            get
            {
                if (confirmCommand == null)
                {
                    confirmCommand = new Command(ConfirmAsync);
                }

                return confirmCommand;
            }
        }

        /// <summary>
        /// Подтверждает причину аварийного завершения.
        /// </summary>
        private async void ConfirmAsync(object commandParameter)
        {
            var activity = (App.Current as App).Activity;
            await Task.Run(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    context.UserActivity
                    .Find(activity.ID).Reason = Reason;
                    if (SoftwareCrash != null && (bool)SoftwareCrash)
                    {
                        context.UserActivity
                    .Find(activity.ID).CrashTypeID = 1;
                    }
                    else if (SystemCrash != null && (bool)SystemCrash)
                    {
                        context.UserActivity
                    .Find(activity.ID).CrashTypeID = 2;
                    }
                    context.SaveChanges();
                }
            });
            FeedbackService.Inform("Спасибо за информацию");
            CloseAction();
        }

        private string reason;

        public string Reason
        {
            get => reason;
            set => SetProperty(ref reason, value);
        }
    }
}
