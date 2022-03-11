using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp_Session2.Models.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp_Session2.ViewModels
{
    public class FlightScheduleViewModel : BaseViewModel
    {
        public FlightScheduleViewModel()
        {
            Title = "Manage Flight Schedules";
            Airports = new ObservableCollection<Airport>
            {
                new Airport
                {
                    IATACode = "Все аэропорты"
                }
            };
            GetAirports()
                .ToList()
                .ForEach(a =>
                {
                    Airports.Add(a);
                });
            FromAirport = ToAirport = Airports.FirstOrDefault();

            SortTypes = new List<string>
            {
                "по дате и времени",
                "по цене на билеты эконом класса",
                "по подтверждённым / не подтверждённым"
            };
            CurrentSortType = SortTypes.First();
            FilterSchedules();
        }

        /// <summary>
        /// Получает коллекцию аэропортов.
        /// </summary>
        /// <returns>Коллекция аэропортов.</returns>
        private IEnumerable<Airport> GetAirports()
        {
            using (SessionTwoEntities context = new SessionTwoEntities())
            {
                return context
                    .Airports
                    .ToList();
            }
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

        public ObservableCollection<Airport> Airports
        {
            get;
            set;
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

        private Command filterSchedulesCommand;

        public ICommand FilterSchedulesCommand
        {
            get
            {
                if (filterSchedulesCommand == null)
                {
                    filterSchedulesCommand = new Command(FilterSchedules);
                }

                return filterSchedulesCommand;
            }
        }

        /// <summary>
        /// Фильтрует рейсы.
        /// </summary>
        private void FilterSchedules(object commandParameter = null)
        {
            var currentSchedules = GetSchedules();
            if (FromAirport != ToAirport)
            {
                if (FromAirport?.ID != 0)
                {
                    currentSchedules = currentSchedules.Where(cs =>
                    {
                        return cs.Route.DepartureAirportID == FromAirport.ID;
                    });
                }
                if (ToAirport?.ID != 0)
                {
                    currentSchedules = currentSchedules.Where(cs =>
                    {
                        return cs.Route.ArrivalAirportID == ToAirport.ID;
                    });
                }
            }
            if (OutboundDate != null)
            {
                currentSchedules = currentSchedules
                       .Where(cs => cs.Date == OutboundDate.Value);
            }
            if (!string.IsNullOrWhiteSpace(FlightNumber))
            {
                currentSchedules = currentSchedules.Where(cs =>
                {
                    return cs.FlightNumber.Contains(FlightNumber);
                });
            }
            switch (CurrentSortType)
            {
                case "по дате и времени":
                    currentSchedules = currentSchedules
                        .OrderBy(cs => cs.Date + cs.Time);
                    break;
                case "по цене на билеты эконом класса":
                    currentSchedules = currentSchedules
                        .OrderBy(cs => cs.EconomyPrice);
                    break;
                case "по подтверждённым / не подтверждённым":
                    currentSchedules = currentSchedules
                        .OrderBy(cs => cs.Confirmed);
                    break;
                default:
                    break;
            }
            Schedules = currentSchedules;
        }
    }
}
