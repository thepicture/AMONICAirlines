using System;

namespace AMONICAirlinesDesktopApp.Models.Entities
{
    public partial class User
    {
        public int Age => (int)Math
            .Floor(DateTime
            .Now
            .Subtract(Birthdate.Value).TotalDays / 365);
    }
}
