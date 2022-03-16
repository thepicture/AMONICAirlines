using AMONICAirlinesDesktopApp_Session3.Models.Entities;
using System.Collections.ObjectModel;

namespace AMONICAirlinesDesktopApp_Session3.Models.ReservationModels
{
    public class Reservation
    {
        public Schedules Outbound { get; set; }
        public Schedules Return { get; set; }
        public CabinTypes CabinType { get; set; }
        public ObservableCollection<Tickets> Passengers { get; set; }
        public int MaxNumberOfPassengers { get; set; }
    }
}
