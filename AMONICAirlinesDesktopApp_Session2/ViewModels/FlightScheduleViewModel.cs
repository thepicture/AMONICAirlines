using AMONICAirlinesDesktopApp_Session2.ViewModels;
using AMONICAirlinesDesktopApp_Session2.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AMONICAirlinesDesktopApp_Session2.ViewModels
{
    public class FlightScheduleViewModel : BaseViewModel
    {
        public FlightScheduleViewModel()
        {
            Title = "Manage Flight Schedules";
            Schedules = GetSchedules();
        }

        /// <summary>
        /// Получает коллекцию рейсов.
        /// </summary>
        /// <returns>Коллекция рейсов.</returns>
        private IEnumerable<Schedule> GetSchedules()
        {
            using (SessionTwoEntities context = new SessionTwoEntities())
            {
                return context
                    .Schedules
                    .Include(s => s.Route)
                    .Include(s => s.Route.Airport)
                    .Include(s => s.Route.Airport1)
                    .ToList();
            }
        }

        private IEnumerable<Airport> airports;

        public IEnumerable<Airport> Airports
        {
            get => airports;
            set => SetProperty(ref airports, value);
        }

        private IEnumerable<string> sortTypes;

        public IEnumerable<string> SortTypes
        {
            get => sortTypes;
            set => SetProperty(ref sortTypes, value);
        }

        private Airport fromAirport;

        public Airport FromAirport
        {
            get => fromAirport;
            set => SetProperty(ref fromAirport, value);
        }

        private Airport toAirport;

        public Airport ToAirport
        {
            get => toAirport;
            set => SetProperty(ref toAirport, value);
        }

        private string currentSortType;

        public string CurrentSortType
        {
            get => currentSortType;
            set => SetProperty(ref currentSortType, value);
        }

        private System.DateTime? outboundDate;

        public System.DateTime? OutboundDate
        {
            get => outboundDate;
            set => SetProperty(ref outboundDate, value);
        }

        private string flightNumber;

        public string FlightNumber
        {
            get => flightNumber;
            set => SetProperty(ref flightNumber, value);
        }

        private IEnumerable<Schedule> schedules;

        public IEnumerable<Schedule> Schedules
        {
            get => schedules;
            set => SetProperty(ref schedules, value);
        }
    }
}
