using System;

namespace AMONICAirlinesDesktopApp.Models.Entities
{
    public partial class UserActivity
    {
        public TimeSpan? TimeSpentOnSystem =>
            LogoutDateTime == null
                    ? null
                    : (TimeSpan?)LogoutDateTime.Value.Subtract(LoginDateTime);
    }
}
