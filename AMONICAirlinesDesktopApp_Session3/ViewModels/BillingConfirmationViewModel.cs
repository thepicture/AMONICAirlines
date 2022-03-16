using AMONICAirlinesDesktopApp_Session3.Models.ReservationModels;
using AMONICAirlinesDesktopApp_Session3.ViewModels;

namespace AMONICAirlinesDesktopApp_Session3.ViewModels
{
    public class BillingConfirmationViewModel : BaseViewModel
    {
        private Reservation reservation;

        public BillingConfirmationViewModel(Reservation reservation)
        {
            Title = "Billing Confirmation";
            Reservation = reservation;
        }

        public Reservation Reservation
        {
            get => reservation;
            set => SetProperty(ref reservation, value);
        }
    }
}