using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp_Session2.Models.Entities;
using System;
using System.Text;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp_Session2.ViewModels
{
    public class ScheduleEditViewModel : BaseViewModel
    {
        private Schedule schedule;

        public ScheduleEditViewModel(Schedule schedule)
        {
            Title = "Schedule edit";
            Schedule = schedule;
            ScheduleDate = Schedule.Date;
            ScheduleTime = Schedule.Time.ToString(@"hh\:mm");
            EconomyPrice = Schedule.EconomyPrice.ToString("F0");
        }

        public Schedule Schedule
        {
            get => schedule;
            set => SetProperty(ref schedule, value);
        }

        private DateTime? scheduleDate;

        public DateTime? ScheduleDate
        {
            get => scheduleDate;
            set => SetProperty(ref scheduleDate, value);
        }

        private string scheduleTime;

        public string ScheduleTime
        {
            get => scheduleTime;
            set => SetProperty(ref scheduleTime, value);
        }

        private string economyPrice;

        public string EconomyPrice
        {
            get => economyPrice;
            set => SetProperty(ref economyPrice, value);
        }

        private Command updateCommand;

        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new Command(Update, CanUpdate);
                }

                return updateCommand;
            }
        }

        /// <summary>
        /// Определяет, может ли рейс быть изменён.
        /// </summary>
        /// <returns><see langword="true"/>, если рейс может быть 
        /// изменён, иначе <see langword="false"/>.</returns>
        private bool CanUpdate(object arg)
        {
            StringBuilder errors = new StringBuilder();
            if (ScheduleDate == null)
            {
                errors.AppendLine("Дата рейса " +
                    "должна быть указана");
            }
            if (string.IsNullOrWhiteSpace(ScheduleTime)
                || !TimeSpan.TryParse(scheduleTime, out _))
            {
                errors.AppendLine("Время рейса - " +
                    "это строка в формате <часы>:<минуты>");
            }
            if (string.IsNullOrWhiteSpace(EconomyPrice)
                || !int.TryParse(EconomyPrice, out _))
            {
                errors.AppendLine("Цена билета эконом-класса - " +
                    "это положительное целое число");
            }
            if (errors.Length > 0)
            {
                Errors = errors.ToString();
            }
            else
            {
                Errors = null;
            }
            return errors.Length == 0;
        }

        private void Update(object commandParameter)
        {
            using (SessionTwoEntities context = new SessionTwoEntities())
            {
                context.Schedules.Find(Schedule.ID).Date = ScheduleDate.Value;
                context.Schedules.Find(Schedule.ID).Time = TimeSpan
                    .Parse(ScheduleTime);
                context.Schedules.Find(Schedule.ID).EconomyPrice = int
                    .Parse(EconomyPrice);
                _ = context.SaveChanges();
            }
            FeedbackService.Inform("Изменения сохранены");
            CloseAction();
        }

        private Command cancelCommand;
        private object errors;

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

        public object Errors
        {
            get => errors;
            set => SetProperty(ref errors, value);
        }

        /// <summary>
        /// Отменяет редактирование рейса, если пользователь 
        /// согласен на отмену.
        /// </summary>
        private void Cancel(object commandParameter)
        {
            if (FeedbackService.Ask("Отменить редактирование рейса?"))
            {
                CloseAction();
            }
        }
    }
}