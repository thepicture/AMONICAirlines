namespace AMONICAirlinesDesktopApp_Session2.Models.Entities
{
    public partial class Schedule
    {
        public decimal BusinessPrice => EconomyPrice * (decimal)1.35;
        public decimal FirstClassPrice => BusinessPrice * (decimal)1.30;
    }
}
