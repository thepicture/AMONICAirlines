using AMONICAirlinesDesktopApp_Session3.Commands;
using AMONICAirlinesDesktopApp_Session3.Models.Entities;
using AMONICAirlinesDesktopApp_Session3.Models.ReservationModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp_Session3.ViewModels
{
    public class BookingConfirmationViewModel : BaseViewModel
    {

        private Reservation reservation;

        public List<Countries> CountriesList
        {
            get => countriesList;
            set => SetProperty(ref countriesList, value);
        }
        public Reservation Reservation
        {
            get => reservation;
            set => SetProperty(ref reservation, value);
        }

        private Countries passportCountry;

        public Countries PassportCountry
        {
            get => passportCountry;
            set => SetProperty(ref passportCountry, value);
        }

        private Command addPassengerCommand;

        public ICommand AddPassengerCommand
        {
            get
            {
                if (addPassengerCommand == null)
                {
                    addPassengerCommand = new Command(AddPassenger,
                        CanAddPassengerExecute);
                }

                return addPassengerCommand;
            }
        }

        /// <summary>
        /// Определяет, можно ли добавить пассажира.
        /// </summary>
        private bool CanAddPassengerExecute(object arg)
        {
            if (string.IsNullOrWhiteSpace(CurrentTicket.Firstname)
                || CurrentTicket.Firstname.Length > 50)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurrentTicket.Lastname)
                || CurrentTicket.Lastname.Length > 50)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(CurrentTicket.Phone)
                || CurrentTicket.Phone.Length > 14)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(CurrentTicket.PassportNumber)
                || CurrentTicket.PassportNumber.Length > 9)
            {
                return false;
            }

            if (CurrentTicket.Users.Birthdate == null
                || (DateTime.Now
                    - CurrentTicket.Users.Birthdate.Value)
                    .TotalDays / 365 < 18)
            {
                return false;
            }

            if (PassportCountry == null)
            {
                return false;
            }

            return true;
        }


        private Tickets currentTicket = new Tickets
        {
            Users = new Users()
        };

        public Tickets CurrentTicket
        {
            get => currentTicket;
            set => SetProperty(ref currentTicket, value);
        }

        /// <summary>
        /// Добавляет пассажира.
        /// </summary>
        private void AddPassenger(object commandParameter)
        {
            CurrentTicket.Users.RoleID = 2;
            CurrentTicket.PassportCountryID = PassportCountry.ID;
            CurrentTicket.Users.LastName = CurrentTicket.Lastname;
            CurrentTicket.Users.Email =
                $"{CurrentTicket.Lastname}" +
                $"{currentTicket.Firstname}" +
                $"@example.com";
            CurrentTicket.Users.Password = Guid
                .NewGuid()
                .ToString();
            CurrentTicket.ScheduleID = Reservation.Outbound.ID;
            CurrentTicket.CabinTypeID = Reservation.CabinType.ID;
            string firstPartOfReference = string.Join("", CurrentTicket
                .Firstname
                .Take(1))
                .ToUpper();
            string secondPartOfReference = string
                .Join("",
                      CurrentTicket.Lastname.Take(5))
                .ToUpper();
            CurrentTicket.BookingReference =
                $"{firstPartOfReference}" +
                $"{secondPartOfReference}";
            CurrentTicket.Confirmed = true;
            Reservation.Passengers.Add(CurrentTicket);
            CurrentTicket = new Tickets
            {
                Users = new Users()
            };
            FeedbackService.Inform("Пассажир успешно добавлен");
        }

        private List<Countries> countriesList;

        public BookingConfirmationViewModel(Reservation reservation)
        {
            Title = "Booking confirmation";
            CountriesList = Task.Run(() =>
            {
                using (SessionThreeEntities context =
                new SessionThreeEntities())
                {
                    return context
                    .Countries
                    .ToList();
                }
            }).Result;
            PassportCountry = CountriesList.FirstOrDefault();
            Reservation = reservation;
        }

        private Command backToSearchCommand;

        public ICommand BackToSearchCommand
        {
            get
            {
                if (backToSearchCommand == null)
                {
                    backToSearchCommand =
                        new Command(BackToSearch);
                }

                return backToSearchCommand;
            }
        }

        /// <summary>
        /// Возвращает пользователя на окно поиска.
        /// </summary>
        private void BackToSearch(object commandParameter)
        {
            if (FeedbackService.Ask("Точно отменить " +
                "бронирование " +
                "и вернуться на окно поиска?"))
            {
                CloseAction();
            }
        }

        private Command confirmBookingCommand;

        public ICommand ConfirmBookingCommand
        {
            get
            {
                if (confirmBookingCommand == null)
                {
                    confirmBookingCommand =
                        new Command(ConfirmBooking,
                                    CanConfirmBookingExecute);
                }

                return confirmBookingCommand;
            }
        }

        /// <summary>
        /// Определяет, можно ли бронировать билет.
        /// </summary>
        private bool CanConfirmBookingExecute(object arg)
        {
            return Reservation.Passengers.Count() > 0;
        }

        /// <summary>
        /// Бронирует билет.
        /// </summary>
        private void ConfirmBooking(object commandParameter)
        {
            try
            {
                using (SessionThreeEntities context =
                    new SessionThreeEntities())
                {
                    Reservation
                        .Passengers
                        .ToList()
                        .ForEach(t => context.Tickets.Add(t));
                    _ = context.SaveChanges();
                }
                CloseAction();
                FeedbackService.Inform("Вы успешно забронировали билет");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                FeedbackService.InformError("Не удалось " +
                    "добавить пассажира. " +
                    "Проверьте подключение к сети");
            }
        }

        private Command removePassengerCommand;

        public ICommand RemovePassengerCommand
        {
            get
            {
                if (removePassengerCommand == null)
                {
                    removePassengerCommand = new Command(RemovePassenger,
                        CanRemovePassengerExecute);
                }

                return removePassengerCommand;
            }
        }

        /// <summary>
        /// Определяет, можно ли удалить 
        /// данные о пассажире.
        /// </summary>
        private bool CanRemovePassengerExecute(object arg)
        {
            return CurrentPassenger != null;
        }

        /// <summary>
        /// Удаляет данные о пассажире.
        /// </summary>
        private void RemovePassenger(object commandParameter)
        {
            if (FeedbackService.Ask("Удалить " +
                "данные о пассажире?"))
            {
                if (Reservation.Passengers.Remove(CurrentPassenger))
                {
                    FeedbackService.Inform("Данные о пассажире удалены");
                }
                else
                {
                    FeedbackService.InformError("Данные о пассажире " +
                        "не удалены. Перезапустите приложение");
                }
            }
        }

        private Tickets currentPassenger;

        public Tickets CurrentPassenger
        {
            get => currentPassenger;
            set => SetProperty(ref currentPassenger, value);
        }
    }
}