using AMONICAirlinesDesktopApp_Session3.Commands;
using AMONICAirlinesDesktopApp_Session3.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp_Session3.ViewModels
{
    public class BookingConfirmationViewModel : BaseViewModel
    {

        private object reservation;

        public object Reservation
        {
            get => reservation;
            set => SetProperty(ref reservation, value);
        }

        private string firstName;

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string lastName;

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private System.DateTime? birthDate;

        public System.DateTime? BirthDate
        {
            get => birthDate;
            set => SetProperty(ref birthDate, value);
        }

        private string passportNumber;

        public string PassportNumber
        {
            get => passportNumber;
            set => SetProperty(ref passportNumber, value);
        }

        private object passportCountry;

        public object PassportCountry
        {
            get => passportCountry;
            set => SetProperty(ref passportCountry, value);
        }

        private string phone;

        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        private Command addPassengerCommand;

        public ICommand AddPassengerCommand
        {
            get
            {
                if (addPassengerCommand == null)
                {
                    addPassengerCommand = new Command(AddPassenger);
                }

                return addPassengerCommand;
            }
        }

        private void AddPassenger(object commandParameter)
        {
        }

        private IEnumerable<Tickets> passengersList;

        public BookingConfirmationViewModel()
        {
            Title = "Booking confirmation";
            PassengersList = Task.Run(() =>
            {
                using (SessionThreeEntities context =
                new SessionThreeEntities())
                {
                    return context
                    .Tickets
                    .Include(t => t.Users)
                    .Include(t => t.CabinTypes)
                    .ToList();
                }
            }).Result;
        }

        public IEnumerable<Tickets> PassengersList
        {
            get => passengersList;
            set => SetProperty(ref passengersList, value);
        }
    }
}