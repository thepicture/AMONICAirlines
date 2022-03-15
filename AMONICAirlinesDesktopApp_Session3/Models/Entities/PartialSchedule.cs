using AMONICAirlinesDesktopApp_Session3.Models.DistanceFinderModels;
using System;
using System.ComponentModel;

namespace AMONICAirlinesDesktopApp_Session3.Models.Entities
{
    public partial class Schedules : INotifyPropertyChanged
    {
        private string flightNumbers;
        private IDistanceFinder<string> finder = new ScheduleDistanceFinder();

        public decimal BusinessPrice => EconomyPrice * (decimal)1.35;
        public decimal FirstClassPrice => BusinessPrice * (decimal)1.30;
        public string FlightNumbers
        {
            get
            {
                finder.GetNumberOfStops(Routes.Airports.IATACode,
                                        Routes.Airports1.IATACode,
                                        DateTime.Parse("1970-01-01"));
                return finder.ToString();
            }

            set
            {
                flightNumbers = value;
                PropertyChanged?
                    .Invoke(this,
                    new PropertyChangedEventArgs(nameof(FlightNumbers)));
            }
        }
        public int NumberOfStops
        {
            get
            {
                return finder.GetNumberOfStops(Routes.Airports.IATACode,
                                               Routes.Airports1.IATACode,
                                               DateTime.Parse("1970-01-01"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
