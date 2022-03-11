using AMONICAirlinesDesktopApp.ViewModels;

namespace AMONICAirlinesDesktopApp_Session2.ViewModels
{
    public class FlightScheduleViewModel : BaseViewModel
    {
        public FlightScheduleViewModel()
        {
            Title = "Manage Flight Schedules";
        }

        private System.Collections.IEnumerable airports;

        public System.Collections.IEnumerable Airports { get => airports; set => SetProperty(ref airports, value); }

        private System.Collections.IEnumerable sortTypes;

        public System.Collections.IEnumerable SortTypes { get => sortTypes; set => SetProperty(ref sortTypes, value); }

        private object fromAirport;

        public object FromAirport { get => fromAirport; set => SetProperty(ref fromAirport, value); }

        private object toAirport;

        public object ToAirport { get => toAirport; set => SetProperty(ref toAirport, value); }

        private object currentSortType;

        public object CurrentSortType { get => currentSortType; set => SetProperty(ref currentSortType, value); }

        private System.DateTime? outboundDate;

        public System.DateTime? OutboundDate { get => outboundDate; set => SetProperty(ref outboundDate, value); }

        private string flightNumber;

        public string FlightNumber { get => flightNumber; set => SetProperty(ref flightNumber, value); }

        private System.Collections.IEnumerable schedules;

        public System.Collections.IEnumerable Schedules { get => schedules; set => SetProperty(ref schedules, value); }
    }
}
