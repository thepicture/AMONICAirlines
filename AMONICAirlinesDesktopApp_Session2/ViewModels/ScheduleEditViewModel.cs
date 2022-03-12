using AMONICAirlinesDesktopApp_Session2.Models.Entities;

namespace AMONICAirlinesDesktopApp_Session2.ViewModels
{
    public class ScheduleEditViewModel : BaseViewModel
    {
        private Schedule schedule;

        public ScheduleEditViewModel(Schedule schedule)
        {
            Title = "Schedule edit";
            Schedule = schedule;
            ScheduleDate = Schedule.Date;
            ScheduleTime = Schedule.Time.ToString(@"hh\:mm");
            EconomyPrice = Schedule.EconomyPrice.ToString("F0");
        }

        public Schedule Schedule
        {
            get => schedule;
            set => SetProperty(ref schedule, value);
        }

        private System.DateTime? scheduleDate;

        public System.DateTime? ScheduleDate
        {
            get => scheduleDate;
            set => SetProperty(ref scheduleDate, value);
        }

        private string scheduleTime;

        public string ScheduleTime
        {
            get => scheduleTime;
            set => SetProperty(ref scheduleTime, value);
        }

        private string economyPrice;

        public string EconomyPrice
        {
            get => economyPrice;
            set => SetProperty(ref economyPrice, value);
        }
    }
}