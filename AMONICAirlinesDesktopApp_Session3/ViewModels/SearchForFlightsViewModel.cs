using AMONICAirlinesDesktopApp_Session3.Commands;
using AMONICAirlinesDesktopApp_Session3.Models.Entities;
using AMONICAirlinesDesktopApp_Session3.Models.ReservationModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp_Session3.ViewModels
{
    public class SearchForFlightsViewModel : BaseViewModel
    {
        public SearchForFlightsViewModel()
        {
            Title = "Search for flights";
            Airports = Task.Run(() =>
            {
                using (SessionThreeEntities context = new SessionThreeEntities())
                {
                    return context.Airports.ToList();
                }
            }).Result;
            FromAirport = Airports.First(a => a.IATACode == "ADE");
            ToAirport = Airports.First(a => a.IATACode == "CAI");
            OutboundDate = DateTime.Parse("2017/10/25");
            ReturnDate = DateTime.Parse("2017/10/26");

            CabinTypesEnumerable = Task.Run(() =>
            {
                using (SessionThreeEntities context = new SessionThreeEntities())
                {
                    return context.CabinTypes.ToList();
                }
            }).Result;
            CurrentCabinType = CabinTypesEnumerable.First();
            GetFlightsAccordingToFilters();
        }

        private IEnumerable<CabinTypes> cabinTypesEnumerable;

        public IEnumerable<CabinTypes> CabinTypesEnumerable
        {
            get => cabinTypesEnumerable;
            set => SetProperty(ref cabinTypesEnumerable, value);
        }

        private CabinTypes currentCabinType;

        public CabinTypes CurrentCabinType
        {
            get => currentCabinType;
            set => SetProperty(ref currentCabinType, value);
        }

        private IEnumerable<Airports> airports;

        public IEnumerable<Airports> Airports
        {
            get => airports;
            set => SetProperty(ref airports, value);
        }

        private Airports fromAirport;

        public Airports FromAirport
        {
            get => fromAirport;
            set => SetProperty(ref fromAirport, value);
        }

        private Airports toAirport;

        public Airports ToAirport
        {
            get => toAirport;
            set => SetProperty(ref toAirport, value);
        }

        private bool? isReturnType = true;

        public bool? IsReturnType
        {
            get => isReturnType;
            set => SetProperty(ref isReturnType, value);
        }

        private bool? isOnewayType = false;

        public bool? IsOnewayType
        {
            get => isOnewayType;
            set => SetProperty(ref isOnewayType, value);
        }

        private DateTime? returnDate;

        public DateTime? ReturnDate
        {
            get => returnDate;
            set => SetProperty(ref returnDate, value);
        }

        private bool? isOutboundThreeDaysBeforeAndAfter = false;

        public bool? IsOutboundThreeDaysBeforeAndAfter
        {
            get => isOutboundThreeDaysBeforeAndAfter;
            set => SetProperty(ref isOutboundThreeDaysBeforeAndAfter, value);
        }

        private bool? isReturnThreeDaysBeforeAndAfter = true;

        public bool? IsReturnThreeDaysBeforeAndAfter
        {
            get => isReturnThreeDaysBeforeAndAfter;
            set => SetProperty(ref isReturnThreeDaysBeforeAndAfter, value);
        }

        private IEnumerable<Schedules> returnFlights;

        public IEnumerable<Schedules> ReturnFlights
        {
            get => returnFlights;
            set => SetProperty(ref returnFlights, value);
        }

        private IEnumerable<Schedules> outboundFlights;

        public IEnumerable<Schedules> OutboundFlights
        {
            get => outboundFlights;
            set => SetProperty(ref outboundFlights, value);
        }

        private string numberOfPassengers;

        public string NumberOfPassengers
        {
            get => numberOfPassengers;
            set => SetProperty(ref numberOfPassengers, value);
        }

        private Command bookFlightCommand;

        public ICommand BookFlightCommand
        {
            get
            {
                if (bookFlightCommand == null)
                {
                    bookFlightCommand = new Command(BookFlight,
                                                    CanBookFlightExecute);
                }

                return bookFlightCommand;
            }
        }

        /// <summary>
        /// Определяет, может ли бронирование рейса совершиться.
        /// </summary>
        private bool CanBookFlightExecute(object arg)
        {
            if (string.IsNullOrWhiteSpace(NumberOfPassengers)
                || !int.TryParse(NumberOfPassengers, out _))
            {
                return false;
            }
            if (currentOutboundFlight == null)
            {
                return false;
            }
            if (currentReturnFlight == null
                && IsReturnType.HasValue
                && !IsReturnType.Value)
            {
                return false;
            }
            if (OutboundDate >= ReturnDate)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Бронирует полёт.
        /// </summary>
        private void BookFlight(object commandParameter)
        {
            var reservation = new Reservation
            {
                CabinType = CurrentCabinType,
                Outbound = CurrentOutboundFlight,
                Return = currentReturnFlight,
                Passengers = new ObservableCollection<Tickets>(),
            };
            WindowService.ShowModalWindowWithParameter
                <BookingConfirmationViewModel, Reservation>(reservation);
        }

        private Command exitCommand;

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new Command(Exit);
                }

                return exitCommand;
            }
        }

        /// <summary>
        /// Выключает приложение.
        /// </summary>
        private void Exit(object commandParameter)
        {
            if (FeedbackService.Ask("Выключить приложение?"))
            {
                App.Current.Shutdown();
            }
        }

        private Command applyCommand;

        public ICommand ApplyCommand
        {
            get
            {
                if (applyCommand == null)
                {
                    applyCommand = new Command(GetFlightsAccordingToFilters, CanApplyExecute);
                }

                return applyCommand;
            }
        }

        /// <summary>
        /// Определяет, можно ли применить фильтрацию.
        /// </summary>
        /// <returns><see langword="true"/>, 
        /// если фильтрация применима, 
        /// иначе <see langword="false"/>.</returns>
        private bool CanApplyExecute(object arg)
        {
            if (OutboundDate == null)
            {
                return false;
            }
            if (FromAirport?.ID == ToAirport?.ID)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Применяет фильтры.
        /// </summary>
        private void GetFlightsAccordingToFilters(object commandParameter = null)
        {
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                IEnumerable<Schedules> currentFlights = context
                    .Schedules
                    .Include(s => s.Routes)
                    .Include(s => s.Routes.Airports)
                    .Include(s => s.Routes.Airports1)
                    .Where(s => s.Routes.DepartureAirportID == FromAirport.ID)
                    .ToList();
                if (IsOutboundThreeDaysBeforeAndAfter.HasValue
                    && IsOutboundThreeDaysBeforeAndAfter.Value)
                {
                    currentFlights = currentFlights
                        .Where(f =>
                        {
                            return Math.Abs(
                                (OutboundDate - f.Date)
                                .Value
                                .Days) <= 3;
                        })
                        .ToList();
                }
                else
                {
                    currentFlights = currentFlights
                     .Where(f =>
                     {
                         return f.Date == OutboundDate;
                     })
                     .ToList();
                }
                OutboundFlights = currentFlights;
            }

            if (IsReturnType.HasValue && !IsReturnType.Value)
            {
                return;
            }
            using (SessionThreeEntities context =
             new SessionThreeEntities())
            {
                IEnumerable<Schedules> currentFlights = context
                    .Schedules
                    .Include(s => s.Routes)
                    .Include(s => s.Routes.Airports)
                    .Include(s => s.Routes.Airports1)
                    .Where(s => s.Routes.DepartureAirportID == ToAirport.ID)
                    .Where(s => s.Routes.ArrivalAirportID == FromAirport.ID)
                    .ToList();
                if (IsReturnThreeDaysBeforeAndAfter.HasValue
                    && IsReturnThreeDaysBeforeAndAfter.Value)
                {
                    currentFlights = currentFlights
                        .Where(f =>
                        {
                            return Math.Abs(
                                (ReturnDate - f.Date)
                                .Value
                                .Days) <= 3;
                        })
                        .ToList();
                }
                else
                {
                    currentFlights = currentFlights
                   .Where(f =>
                   {
                       return f.Date == ReturnDate;
                   })
                   .ToList();
                }
                ReturnFlights = currentFlights;
            }
        }

        private DateTime? outboundDate;

        public DateTime? OutboundDate
        {
            get => outboundDate;
            set => SetProperty(ref outboundDate, value);
        }

        private Schedules currentOutboundFlight;

        public Schedules CurrentOutboundFlight
        {
            get => currentOutboundFlight;
            set => SetProperty(ref currentOutboundFlight, value);
        }

        private Schedules currentReturnFlight;

        public Schedules CurrentReturnFlight
        {
            get => currentReturnFlight;
            set => SetProperty(ref currentReturnFlight, value);
        }
    }
}