using System;

namespace AMONICAirlinesDesktopApp.ViewModels
{
    public class TrackingViewModel : BaseViewModel
    {
        public TrackingViewModel()
        {
            Title = "No logout detected";
        }

        private DateTime lastLogin;

        public DateTime LastLogin
        {
            get => lastLogin;
            set => SetProperty(ref lastLogin, value);
        }
    }
}
