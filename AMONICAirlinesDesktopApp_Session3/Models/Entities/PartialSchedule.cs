namespace AMONICAirlinesDesktopApp_Session3.Models.Entities
{
    public partial class Schedules
    {
        public decimal BusinessPrice => EconomyPrice * (decimal)1.35;
        public decimal FirstClassPrice => BusinessPrice * (decimal)1.30;
    }
}
