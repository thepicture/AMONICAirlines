using AMONICAirlinesDesktopApp_Session3.Commands;
using AMONICAirlinesDesktopApp_Session3.Models.Entities;
using AMONICAirlinesDesktopApp_Session3.Models.ReservationModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp_Session3.ViewModels
{
    public class BillingConfirmationViewModel : BaseViewModel
    {
        private Reservation reservation;

        public BillingConfirmationViewModel(Reservation reservation)
        {
            Title = "Billing Confirmation";
            Reservation = reservation;
            TotalAmount = Task.Run(() =>
            {
                using (SessionThreeEntities context =
                new SessionThreeEntities())
                {
                    decimal currentSum = 0;
                    foreach (var ticket in Reservation.Passengers)
                    {
                        currentSum += context.Tickets
                        .Find(ticket.ID).Schedules.EconomyPrice;
                    }
                    if (Reservation.CabinType.Name == "Business")
                    {
                        currentSum *= (decimal)1.35;
                    }
                    else if (Reservation.CabinType.Name == "First Class")
                    {
                        currentSum *= (decimal)1.35;
                        currentSum *= (decimal)1.30;
                    }
                    return currentSum;
                }
            }).Result;
        }

        public Reservation Reservation
        {
            get => reservation;
            set => SetProperty(ref reservation, value);
        }

        private bool? creditCardPaymentType = true;

        public bool? CreditCardPaymentType
        {
            get => creditCardPaymentType;
            set => SetProperty(ref creditCardPaymentType, value);
        }

        private bool? cashPaymentType = false;

        public bool? CashPaymentType
        {
            get => cashPaymentType;
            set => SetProperty(ref cashPaymentType, value);
        }

        private bool? voucherPaymentType = false;

        public bool? VoucherPaymentType
        {
            get => voucherPaymentType;
            set => SetProperty(ref voucherPaymentType, value);
        }

        private Command cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new Command(Cancel);
                }

                return cancelCommand;
            }
        }

        private void Cancel(object commandParameter)
        {
            if (FeedbackService.Ask("Вернуться к подтверждению бронирования?"))
            {
                CloseAction();
            }
        }

        private Command issueTicketsCommand;
        private decimal totalAmount;

        public ICommand IssueTicketsCommand
        {
            get
            {
                if (issueTicketsCommand == null)
                {
                    issueTicketsCommand = new Command(IssueTickets);
                }

                return issueTicketsCommand;
            }
        }

        public decimal TotalAmount
        {
            get => totalAmount;
            set => SetProperty(ref totalAmount, value);
        }

        /// <summary>
        /// Осуществляет подтверждение бронирования билета.
        /// </summary>
        private void IssueTickets(object commandParameter)
        {
            try
            {
                using (SessionThreeEntities context =
                      new SessionThreeEntities())
                {
                    foreach (var ticket in Reservation.Passengers)
                    {
                        context.Tickets.Find(ticket.ID).Confirmed = true;
                        _ = context.SaveChanges();
                    }
                }
                FeedbackService.Inform("Резервирование " +
                    "успешно подтверждено");
                foreach (Window ownedWindow
                    in App.Current.MainWindow.OwnedWindows)
                {
                    ownedWindow.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                FeedbackService.InformError("Не удалось " +
                    "забронировать билет. " +
                    "Перезапустите окно и " +
                    "попробуйте ещё раз");
            }
        }
    }
}